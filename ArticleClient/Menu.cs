using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArticleClient.ServiceReference1;

namespace ArticleClient
{
    class Menu
    {
        List<Article> articlesList = new List<Article>();
        int billNumber = 0;


        public Menu()
        {
            using (ServiceReference1.ArticleServiceClient wcf = new ServiceReference1.ArticleServiceClient())
            {
                foreach (var article in wcf.GetArticles())
                {
                    articlesList.Add(article);
                }

            }

        }
        public void MainMenu()
        {
            string choice;
            bool endApp = false;
            do
            {
                Console.WriteLine("\n1.Prikazi sve artikle");
                Console.WriteLine("2.Kreiraj racun");
                Console.WriteLine("3.Izmeni cene");
                Console.WriteLine("4.Dodaj novi artikal");
                Console.WriteLine("5.Izlaz");

                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":

                        foreach (var article in articlesList)
                        {
                            Console.WriteLine(article.Name + " $" + article.Price + " " + article.Amount);
                        }
                        break;
                    case "2":
                        CreateBill();
                        break;
                    case "3":
                        UpdateArticle();
                        break;
                    case "4":
                        AddArticle();
                        break;
                    case "5":
                        endApp = true;
                        break;
                    default:
                        Console.WriteLine("Ne postoji ovaj izbor");
                        break;
                }
            } while (!endApp);

        }

        private void AddArticle()
        {
            Console.WriteLine("Unesite ime artikla:");
            string name = Console.ReadLine();
            Console.WriteLine("Unesite cenu:");
            decimal price = DecimalInput();
            Console.WriteLine("Unesite kolicinu:");
            int amount = IntInput();

            Article lastArticle = articlesList[articlesList.Count - 1];
            string id = lastArticle.ID;

            Article article = new Article();
            article.ID = id;
            article.Price = price;
            article.Name = name;
            article.Amount = amount;

            articlesList.Add(article);

            using (ArticleServiceClient wcf = new ArticleServiceClient())
            {
                wcf.UpdateArticles(articlesList.ToArray());
            }



        }

        private void UpdateArticle()
        {

            ShowAllArticles();
            do
            {
                Console.WriteLine("\nUnesite broj artikla");
                string articleID = Console.ReadLine();

                Article article = GetArticleById(articleID);
                if (article == null)
                {
                    Console.WriteLine("Ne postoji artikal sa ovim ID");
                    continue;
                }
                Console.WriteLine("Unesite novu cenu:");
                int price = -1;
                do
                {
                    price = IntInput();

                    if (price <= 0)
                    {
                        Console.WriteLine("Cene mora biti veca od 0");
                    }
                } while (price <= 0);

                article.Price = price;



                Console.WriteLine("Da li zelite da menjate jos artikala d/n");
                string answer = Console.ReadLine();

                if (answer.Equals("d") || answer.Equals("D"))
                {

                }
                else
                {
                    break;
                }


            } while (true);

            using (ArticleServiceClient wcf = new ArticleServiceClient())
            {
                wcf.UpdateArticles(articlesList.ToArray());
            }
        }

        void CreateBill()
        {
            List<Article> articlesForBill = new List<Article>();
            ShowAllArticles();
            do
            {
                Console.WriteLine("\nUnesite broj artikla");
                string articleID = Console.ReadLine();

                Article article = GetArticleById(articleID);
                if (article == null)
                {
                    Console.WriteLine("Ne postoji artikal sa ovim ID");
                    continue;
                }
                Console.WriteLine("Unesite kolicinu:");
                int amount = -1;
                do
                {
                    amount = IntInput();
                    if (amount > article.Amount)
                    {
                        Console.WriteLine("Nema dovoljno na stanju. Unesite manju kolicinu");
                    }
                    if (amount <= 0)
                    {
                        Console.WriteLine("Kolicina mora biti veca od 0");
                    }
                } while (amount > article.Amount || amount <= 0);

                article.Amount -= amount;

                Article articleForBill = new Article();
                articleForBill.Name = article.Name;
                articleForBill.Amount = amount;
                articleForBill.Price = article.Price;

                string str = string.Format("Ukupan iznos: {0}", articleForBill);
                articlesForBill.Add(articleForBill);

                Console.WriteLine("Da li zelite da dodate jos artikala d/n");
                string answer = Console.ReadLine();

                if (answer.Equals("d") || answer.Equals("D"))
                {

                }
                else
                {
                    break;
                }


            } while (true);

            
            StringBuilder sb = new StringBuilder();
            DateTime dateTime = DateTime.Now;
            sb.Append(dateTime.Day);
            sb.Append("/");
            sb.Append(dateTime.Month);
            sb.Append("/");
            sb.Append(dateTime.Year);
            sb.Append(" ");
            sb.Append(dateTime.Hour);
            sb.Append(":");
            sb.Append(dateTime.Minute);
            sb.Append(":");
            sb.Append(dateTime.Second);

            sb.Append(",");
            //texToFile.Add(sb.ToString());



            decimal totalAmount = 0;

            foreach (var item in articlesForBill)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(item.Name);
                stringBuilder.Append(" - ");
                string str = string.Format("{0}*{1}", item.Amount, item.Price);
                stringBuilder.Append(str);

                sb.Append(stringBuilder.ToString());
                sb.Append(",");

                totalAmount += item.Amount * item.Price;
            }

            string totAmount = string.Format("Ukupan iznos: {0}", totalAmount);
            sb.Append(totAmount);


            StringBuilder stringBuilder1 = new StringBuilder();
            string currentDate = DateTime.Now.ToString("ddMMyyyyHHmmss");

            stringBuilder1.Append("Racun_");
            stringBuilder1.Append(++billNumber);

            stringBuilder1.Append("_");

            stringBuilder1.Append(currentDate);


            Console.WriteLine(stringBuilder1.ToString());

            using (ArticleServiceClient wcf = new ArticleServiceClient())
            {
                wcf.CreateBill(sb.ToString());
                wcf.UpdateArticles(articlesList.ToArray());
            }

        }

        void ShowAllArticles()
        {
            List<Article> articles = new List<Article>();
            using (ArticleServiceClient wcf = new ArticleServiceClient())
            {
                foreach (var article in wcf.GetArticles())
                {
                    articles.Add(article);
                }

            }



            foreach (var article in articles)
            {
                Console.WriteLine(article.ID + " " + article.Name + " $" + article.Price + " " + article.Amount);
            }
        }




        public Article GetArticleById(string ID)
        {
            return articlesList.Where(x => x.ID.Equals(ID)).FirstOrDefault();
        }

        int IntInput()
        {
            int returnValue;
            bool correct;

            do
            {
                correct = Int32.TryParse(Console.ReadLine(), out returnValue);

                if (!correct)
                {

                    Console.WriteLine("Nevalidan unos");
                }
            } while (!correct);

            return returnValue;
        }

        decimal DecimalInput()
        {
            decimal returnValue;
            bool correct;

            do
            {
                correct = Decimal.TryParse(Console.ReadLine(), out returnValue);

                if (!correct)
                {

                    Console.WriteLine("Nevalidan unos");
                }
            } while (!correct);

            return returnValue;
        }
    }
}
