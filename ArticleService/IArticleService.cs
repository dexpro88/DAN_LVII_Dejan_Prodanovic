using ArticleService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ArticleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IArticleService
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        List<Article> GetArticles();

        [OperationContract]
        void CreateBill(string billText);

        [OperationContract]
        void UpdateArticles(List<Article> articles);

    }


     
}
