using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Drawing;

namespace ExtractData
{
    public class DBService
    {
        private string ConnectionString = "Server=127.0.0.1;Database=devcan;User Id=sa;Password=saadmin;";
        string dirPath = @"c:\ExtractedImages";
        List<string> strList;
        public DBService()
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        public void TransferData()
        {
            
            DeleteRecords();
            strList = new List<string>();
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlDataAdapter adp = new SqlDataAdapter("select image_id, application_id, filename, image from old", con);
                adp.Fill(dt);
                SqlCommand com = new SqlCommand("insert into new (image_id, upload_obua_id, document_type_id, upload_date, Record_Create_Date, application_id, last_download_timestamp, last_download_obua_id, description, filename) select image_id, upload_obua_id, document_type_id, upload_date, Record_Create_Date, application_id, last_download_timestamp, last_download_obua_id, description, filename from old with (nolock)", con);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                com.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                
                
                foreach (DataRow dr in dt.Rows)
                {
                    byte[] source = (byte[])dr["image"];
                    string fname = dr["filename"].ToString().Trim();
                    string extension = "";
                    int lio = fname.LastIndexOf(".");
                    if (lio<2)
                    {
                        extension = "";
                    }
                    else
                    {
                        extension=fname.Substring(lio, fname.Length - lio);
                    }
                    if(extension==".do")
                    {
                        string str = fname;
                    }
                    string appId = dr["application_id"].ToString();
                    appId = wordCount("$^" + appId + "^$");
                    
                    File.WriteAllBytes(dirPath + @"\" + appId + extension, source);

                }
            }

        }
        string  wordCount(string word)
        {
            int i = 0;
            foreach(string strcheck in strList)
            {
                if(strcheck==word)
                {
                    i++;
                }
            }
            strList.Add(word);
            if (i == 0)
            {
                return word.Replace("$^", "").Replace("^$", "");
            }
            else
            {
                return word.Replace("$^", "").Replace("^$", "") + "(" + i.ToString() + ")";
            }
        }
        public void DeleteRecords()
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(dirPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand comm = new SqlCommand("Delete from new", con);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                comm.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                    con.Close();

            }
        }
    }
}
