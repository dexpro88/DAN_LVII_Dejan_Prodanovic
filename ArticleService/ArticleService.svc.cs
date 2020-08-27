using ArticleService.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ArticleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IArticleService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        private readonly string path =
           AppDomain.CurrentDomain.BaseDirectory + @"\Articles.txt";
        

        int billNumber = 1;

        public List<Article> GetArticles()
        {
            List<Article> articles = new List<Article>();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    int counter = 1;

                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] strFromFile = line.Split(',');
                        Article article = new Article();
                        article.ID = counter.ToString();
                        counter++;
                        article.Name = strFromFile[0];
                        article.Amount = Int32.Parse(strFromFile[1]);
                        article.Price = Int32.Parse(strFromFile[2]);

                        articles.Add(article);


                    }
                }
                return articles;
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return articles;
            }
        }
        public void CreateBill(string billText)
        {
            try
            {

                StringBuilder sb = new StringBuilder();
                sb.Append(@"");

                DateTime dateTime = DateTime.Now;

                sb.Append("Racun_");
                sb.Append(billNumber);
                billNumber++;
                sb.Append("_");
                string currentDate = DateTime.Now.ToString("ddMMyyyyHHmmss");

                sb.Append(currentDate);

                sb.Append(".txt");

                
                string billPath =
                AppDomain.CurrentDomain.BaseDirectory + sb.ToString();

                 

                string[] toFile = billText.Split(',');
                using (StreamWriter sw = new StreamWriter(billPath))
                {
                    foreach (var item in toFile)
                    {
                        sw.WriteLine(item);
                    }


                }

            }
            catch (Exception e)
            {

                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);

            }
        }

        public void UpdateArticles(List<Article> articles)
        {
            try
            {



                using (StreamWriter sw = new StreamWriter(@"..\..\Articles.txt"))
                {
                    foreach (var item in articles)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(item.Name);
                        sb.Append(",");
                        sb.Append(item.Amount);
                        sb.Append(",");
                        sb.Append(item.Price);

                        sw.WriteLine(sb.ToString());
                    }


                }

            }
            catch (Exception e)
            {

                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);

            }
        }
    }
}
