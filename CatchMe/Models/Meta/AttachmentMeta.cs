using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatchMe.Models
{
    public class attachmentMeta
    {
        public int attachment_id { get; set; }
        public int task_id { get; set; }
        public int user_id { get; set; }
        public string filename { get; set; }
        public string mimetype { get; set; }
        public Nullable<int> content_length { get; set; }
        public Nullable<System.DateTime> created_on { get; set; }
        public bool is_disabled { get; set; }

    }

    [MetadataType(typeof(attachmentMeta))]
    public partial class attachment
    {

        public string extension
        {
            get
            {
                if(filename != null)
                {
                    var idx = filename.IndexOf('.');
                    return filename.Substring(idx);

                }
                return string.Empty;
            }

        } 

        public string size
        {
            get
            {
                if(content_length > 1024 && content_length < (1024*1024))
                {
                    return string.Concat(content_length/1024, " KB");

                }

                if (content_length >= (1024 * 1024))
                {
                    return string.Concat(content_length / (1024*1024), " MB");

                }


                return string.Concat(content_length, " b");
            }
        }
        
        
        public string icon
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(extension))
                {

                    switch (extension.ToLower())
                    {
                        case ".xlsx": return "file-excel-o";
                        case ".pdf": return "file-pdf-o";
                        case ".docx": return "file-word-o";
                        case ".pptx": return "file-powerpoint-o";
                        case ".jpg":
                        case ".jpeg":
                        case ".png":

                            return "file-image-o";
                        case ".txt": return "file-text-o";
                        case ".sql": return "file-code-o";
                        default:
                            break;
                    }
                }

                return "file";
            }
        }  
    }

    

}