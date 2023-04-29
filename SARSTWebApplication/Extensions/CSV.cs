using System.Data;

public static class CSVUtlity
{


    public static byte[] ToCSV(this DataTable dtDataTable)
    {

        using (var memoryStream = new MemoryStream())
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            //headers    
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                streamWriter.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    streamWriter.Write(",");
                }
            }
            streamWriter.Write(streamWriter.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            streamWriter.Write(value);
                        }
                        else
                        {
                            streamWriter.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        streamWriter.Write(",");
                    }
                }
                streamWriter.Write(streamWriter.NewLine);
            }
            streamWriter.Flush();
            return memoryStream.ToArray();
        }
    }
}