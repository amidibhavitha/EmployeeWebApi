using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using MVCcrud.Models;
using System.Text;

namespace MVCcrud.Controllers
    
{
    public class EmployeeController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44318/api");
        HttpClient client;
        public EmployeeController()
        {
            client= new HttpClient();
            client.BaseAddress = baseAddress;
        }
       
        public ActionResult Index()
        {
            List<Employeem> modelList = new List<Employeem>();
           HttpResponseMessage respone=client.GetAsync(client.BaseAddress+ "/Employeem").Result;
            if(respone.IsSuccessStatusCode)
            {
                string data=respone.Content.ReadAsStringAsync().Result;
                modelList=JsonConvert.DeserializeObject<List<Employeem>> (data);
            }
            return View(modelList);
        }
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(Employeem model)
        {
            string data=JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data,Encoding.UTF8 ,"application/json");
            HttpResponseMessage response= client.PostAsync(client.BaseAddress + "/Employeem",content).Result;
            if(response.IsSuccessStatusCode) 
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Edit(int EmpId)
        {
            Employeem model = new Employeem();
            HttpResponseMessage respone = client.GetAsync(client.BaseAddress + "/Employeem"+EmpId).Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<Employeem>(data);
            }
            return View("Create",model);
        }

           
        
        [HttpPost]
        public ActionResult  Edit(Employeem model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Employeem"+model.EmpId, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Create", model);
        }

    }

}