using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Content_Centre_API.Messaging;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;
//using ProjectManagement_API.Models.DTO;
//using Content_Centre_API.Messaging;
using Content_Centre_API_CORS.Messaging;



namespace Importinator
{
    partial class importer : ServiceBase
    {
        
        public importer()
        {
            InitializeComponent();
        }

        public async void import(string[] args)
        {
            //ServiceBase.Run(new ServiceBase[]
            //    {
            //        new importer()
            //    });

            // TODO: Add code here to start your service.
            
            
            // get the specs to be imported
            String lineS = "";
            StreamReader reader = File.OpenText(args[0]);
            lineS = reader.ReadToEnd();

            theList test = new theList();
            test = JsonConvert.DeserializeObject<theList>(lineS);



            foreach (DataList m in test.menuDataList)
            {
                // build code string
                for (int x = 0; x < m.mealList.Count; x++)
                {
                    m.newCode += m.mealList[x].code;
                    if (x + 1 != m.mealList.Count)
                        m.newCode += "/";
                }

                // build class string
                for (int x = 0; x < m.classCodeList.Count; x++)
                {
                    m.newClass += m.classCodeList[x].code;
                    if (x + 1 != m.classCodeList.Count)
                        m.newClass += "/";
                }

                // build station string
                for (int x = 0; x < m.departureArrivalList.Count; x++)
                {
                    m.newStation += m.departureArrivalList[x].departure.code;
                    if (m.departureArrivalList[x].arrival != null)
                        m.newStation += "(" + m.departureArrivalList[x].arrival.code + ")";
                    if (x + 1 != m.departureArrivalList.Count)
                        m.newStation += "/";
                }
                
                m.specMatchKey = m.newCode +
                    m.validityBegin + m.pairingGrid + m.cycle + m.newStation +
                    m.newClass;

                if (m.OSCKey > -1)
                    m.tempSpecID = m.OSCKey;
            }

            //assign temporary specIDs
            int tempSpecCounter = -2;

            for (int y = 0; y < test.menuDataList.Count(); y++)
            {
                if (test.menuDataList[y].tempSpecID == -1)
                {
                    test.menuDataList[y].tempSpecID = tempSpecCounter;

                    bool found = false;
                    for (int x3 = y + 1; x3 < test.menuDataList.Count(); x3++)
                    {
                        //test.menuDataList[x3].tempSpecID = --tempSpecCounter;

                        if (test.menuDataList[x3].tempSpecID == -1)
                        {
                            if (test.menuDataList[y].specMatchKey == test.menuDataList[x3].specMatchKey)
                            {
                                test.menuDataList[x3].tempSpecID = test.menuDataList[y].tempSpecID;
                                found = true;
                            }
                        }
                    }
                    if (found)
                        tempSpecCounter--;
                }
            }

            foreach (DataList m in test.menuDataList)
            {
                Console.WriteLine(m.specMatchKey + " : " + m.tempSpecID);
            }


            // get Token
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            Console.WriteLine("Getting token");

            HttpClient client = new HttpClient();
            string OscToken = "Bearer " + await GetAuthToken();
            OscToken = OscToken.Replace("#access_token=", "");
            client.DefaultRequestHeaders.Add("Authorization", OscToken);

            Console.WriteLine("token Retrieved");

            //using (StreamReader reader = File.OpenText(args[0]))
            //    while (!reader.EndOfStream)
            //    {
            //        //string[] temp;
            //        string line = reader.ReadLine();
            //        if (null == line)
            //            continue;
            //        lineS += line;

            //        reader.
            //    }

            //clientDTO testClient = new clientDTO();
            //projectDTO proJ = new projectDTO();
            //responseObject resultObject = new responseObject();
            //ProjectManagementRequest reQ = new ProjectManagementRequest();

            //testClient.name = "Paul Thorott";
            //testClient.email = "Cloud.Strife@ffxiv.pol";
            //testClient.phoneNumber = "1.123.123.1234";

            //Console.WriteLine("some text....");

            //proJ.client = testClient;
            //proJ.documents = new List<documentDTO>();
            //proJ.resources = new List<resourceDTO>();
            //proJ.invoices = new List<invoiceDTO>();
            //proJ.projectName = "What What?";

            //documentDTO tempDoc = new documentDTO();
            //resourceDTO tempRes = new resourceDTO();
            //invoiceDTO tempInv = new invoiceDTO();
            //languageDTO tempLan = new languageDTO();

            //tempLan.translateDirectionID = new List<int>();
            //tempLan.translateDirectionID.Add(3);
            //tempLan.translateDirectionID.Add(2);            

            //tempDoc.name = "DOC1";
            //tempDoc.filePath = "x:\\sick.jpg";
            //tempDoc.status = 1;
            //tempDoc.sourceLanguageID = 1;
            //tempDoc.translateDirections = new languageDTO();
            //tempDoc.translateDirections = tempLan;


            //tempDoc.typeID = 1;

            //tempRes.firstName = "Don";
            //tempRes.lastName = "Matroska";
            //tempRes.company = "OSC";
            //tempRes.address = "100 disney drive";
            //tempRes.type = "0";
            //tempRes.isCompany = false;
            //tempRes.languageAssociations = new languageDTO();
            //tempRes.languageAssociations = tempLan;
            //tempRes.languageAssociations.translateDirectionID[0] = 1;
            //tempRes.languageAssociations.translateDirectionID[1] = 3;
            //tempRes.documents = new List<documentDTO>();

            //tempInv.price = 0.50;
            //tempInv.status = "1";
            //tempInv.version = 1.00;
            //tempInv.documentID = 344;

            //proJ.invoices.Add(tempInv);
            //proJ.documents.Add(tempDoc);
            //proJ.resources.Add(tempRes);


            //reQ.projects = new List<projectDTO>();

            //reQ.projects.Add(proJ);

            Console.WriteLine("some more text....");

            //String ajaxURI = "https://localhost/ProjectManagement-API/api/projectmanagement/postProject/";



            //HttpResponseMessage httpResponse = client.PostAsJsonAsync<ProjectManagementRequest>(ajaxURI, reQ).Result;
            //resultObject = httpResponse.Content.ReadAsAsync<responseObject>().Result;


            String ajaxURI = ConfigurationManager.AppSettings["SpecWebApi"];

            // -----------Get meal types
            MealTypeTransferObject reQ1 = new MealTypeTransferObject();
            List<MealTypeTransferObject> MealTypes = new List<MealTypeTransferObject>();

            HttpResponseMessage httpResponse = client.PostAsJsonAsync<MealTypeTransferObject>(ajaxURI + "/getAllMealTypes", reQ1).Result;
            Console.WriteLine("\n|--------------------|");
            Console.WriteLine("... Getting Meal Type IDs");
            Console.WriteLine("Data Sent.. did it work? ....");

            if (httpResponse.StatusCode == (HttpStatusCode)200)
            {
                Console.WriteLine("yes....yes it did");
                MealTypeResponseObject PressXtoJSON = new MealTypeResponseObject();
                PressXtoJSON = await httpResponse.Content.ReadAsAsync<MealTypeResponseObject>();
                MealTypes = JsonConvert.DeserializeObject<List<MealTypeTransferObject>>(PressXtoJSON.JsonResults);
                Console.WriteLine("MealTypes populated");
            }
            else
            {
                Console.WriteLine("nope, wrong again!");
                Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
            }
            


            // ---------Get Status Codes
            StatusTransferObject reQ2 = new StatusTransferObject();
            List<StatusTransferObject> SpecStatuses = new List<StatusTransferObject>();

            httpResponse = client.PostAsJsonAsync<StatusTransferObject>(ajaxURI + "/getAllStatuses", reQ2).Result;
            Console.WriteLine("\n|--------------------|");
            Console.WriteLine("... Getting Status IDs");
            Console.WriteLine("Data Sent.. did it work? ....");

            if (httpResponse.StatusCode == (HttpStatusCode)200)
            {
                Console.WriteLine("yes....yes it did");
                StatusResponseObject PressXtoJSON = new StatusResponseObject();
                PressXtoJSON = await httpResponse.Content.ReadAsAsync<StatusResponseObject>();
                SpecStatuses = JsonConvert.DeserializeObject<List<StatusTransferObject>>(PressXtoJSON.JsonResults);
                Console.WriteLine("Statuses populated");
            }
            else
            {
                Console.WriteLine("nope, wrong again!");
                Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
            }

            // -----------Get rotations
            RotationGrillTransferObject reQ3 = new RotationGrillTransferObject();
            List<RotationGrillTransferObject> Rotations = new List<RotationGrillTransferObject>();

            httpResponse = client.PostAsJsonAsync<RotationGrillTransferObject>(ajaxURI + "/getRotationByName", reQ3).Result;
            Console.WriteLine("\n|--------------------|");
            Console.WriteLine("... Rotation Grill IDs");
            Console.WriteLine("Data Sent.. did it work? ....");

            if (httpResponse.StatusCode == (HttpStatusCode)200)
            {
                Console.WriteLine("yes....yes it did");
                RotationGrillResponseObject PressXtoJSON = new RotationGrillResponseObject();
                PressXtoJSON = await httpResponse.Content.ReadAsAsync<RotationGrillResponseObject>();
                Rotations = JsonConvert.DeserializeObject<List<RotationGrillTransferObject>>(PressXtoJSON.JsonResults);
                Console.WriteLine("Rotations populated");
            }
            else
            {
                Console.WriteLine("nope, wrong again!");
                Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
            }

            // -----------Get languages
            LanguageTransferObject reQ4 = new LanguageTransferObject();
            List<LanguageTransferObject> Languages = new List<LanguageTransferObject>();

            httpResponse = client.PostAsJsonAsync<LanguageTransferObject>(ajaxURI + "/getAllLanguages", reQ4).Result;
            Console.WriteLine("\n|--------------------|");
            Console.WriteLine("... Rotation Grill IDs");
            Console.WriteLine("Data Sent.. did it work? ....");

            if (httpResponse.StatusCode == (HttpStatusCode)200)
            {
                Console.WriteLine("yes....yes it did");
                LanguageResponseObject PressXtoJSON = new LanguageResponseObject();
                PressXtoJSON = await httpResponse.Content.ReadAsAsync<LanguageResponseObject>();
                Languages = JsonConvert.DeserializeObject<List<LanguageTransferObject>>(PressXtoJSON.JsonResults);
                Console.WriteLine("Languages populated");
            }
            else
            {
                Console.WriteLine("nope, wrong again!");
                Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
            }

            // -----------Get allergens....
            AllergenTransferObject reQ5 = new AllergenTransferObject();
            reQ5.AccountID = 1;
            //reQ5.LanguageID = 1;

            List<AllergenTransferObject> Allergens = new List<AllergenTransferObject>();

            httpResponse = client.PostAsJsonAsync<AllergenTransferObject>(ajaxURI + "/getAllAllergens", reQ5).Result;
            Console.WriteLine("\n|--------------------|");
            Console.WriteLine("... getting Allergen IDs");
            Console.WriteLine("Data Sent.. did it work? ....");
            //Console.WriteLine("NOPE...... just kidding");

            if (httpResponse.StatusCode == (HttpStatusCode)200)
            {
                Console.WriteLine("yes....yes it did");
                AllergenResponseObject PressXtoJSON = new AllergenResponseObject();
                PressXtoJSON = await httpResponse.Content.ReadAsAsync<AllergenResponseObject>();
                Allergens = JsonConvert.DeserializeObject<List<AllergenTransferObject>>(PressXtoJSON.JsonResults);
                Console.WriteLine("Allergens populated");
            }
            else
            {
                Console.WriteLine("nope, wrong again!");
                Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
            }
            
            //List<AllergenTransferObject> Allergens = new List<AllergenTransferObject>();
            //AllergenTransferObject tempAlly = new AllergenTransferObject();

            //tempAlly.AllergenID = 0;
            //tempAlly.Value = "Fish";
            //Allergens.Add(tempAlly);
            
            //tempAlly.AllergenID = 1;
            //tempAlly.Value = "Egg";
            //Allergens.Add(tempAlly);

            //tempAlly.AllergenID = 2;
            //tempAlly.Value = "Gluten";
            //Allergens.Add(tempAlly);

            //tempAlly.AllergenID = 3;
            //tempAlly.Value = "Bees";
            //Allergens.Add(tempAlly);

            //tempAlly.AllergenID = 4;
            //tempAlly.Value = "Milk";
            //Allergens.Add(tempAlly);

            // ------------------------////////////////////////

            COURSE pole = new COURSE();
            List<ALLERGEN> oool = new List<ALLERGEN>();
            ALLERGEN looo = new ALLERGEN();


            looo.allergen = "milk";
            oool.Add(looo);

            looo.allergen = "Egg";
            oool.Add(looo);

            looo.allergen = "BeeS";
            oool.Add(looo);

            pole.allergens = oool;

            String PressMtoJSON = "";
            //PressMtoJSON = JsonConvert.DeserializeObject<theList>(lineS);
            PressMtoJSON = JsonConvert.SerializeObject(pole);



            // display the specs for verification
            String str = "";
            int start = 0, howMany = 9; //9
            for (int xList = start; xList < howMany + start; xList++)
            {
                DataList temp = new DataList();
                SpecTransferObject newSpec = new SpecTransferObject();

                temp = test.menuDataList[xList];

                //newSpec.AccountID = "1"; // Air France

                newSpec.Rotation = temp.cycle;
                newSpec.ValidDate = temp.validityBegin;
                newSpec.CatererName = temp.caterer;


                //until parsed is fixed

                if (temp.state == null)
                    temp.state = "Proofing";



                // Check if spec exists


                //String codeString = "", stationString = "";

                newSpec.Class = temp.newClass;
                newSpec.Code = temp.newCode;
                newSpec.Station = temp.newStation;


                // get MealType
                newSpec.MealTypeID = getMealTypeID(MealTypes, temp.mealType);

                // get Status Code
                newSpec.StatusID = getStatusID(SpecStatuses, temp.state);

                // Get Rotation ID?
                newSpec.RotationID = getRotationGrillID(Rotations, temp.pairingGrid);

                // ----------- insert NEW and Get Spec ID .......

                //SpecTransferObject reQ5 = new SpecTransferObject();
                //List<LanguageTransferObject> Languages = new List<LanguageTransferObject>();



                



                if (temp.tempSpecID > -1) // check if it exists
                {
                    SpecTransferObject checkSpec = new SpecTransferObject();
                    checkSpec.SpecID = temp.tempSpecID;


                    httpResponse = client.PostAsJsonAsync<SpecTransferObject>(ajaxURI + "/getSpecByID", checkSpec).Result;
                    Console.WriteLine("\n|--------------------|");
                    Console.WriteLine("... Checking if Spec Exists");
                    Console.WriteLine("Data Sent.. did it work? ....");

                    if (httpResponse.StatusCode == (HttpStatusCode)200)
                    {
                        Console.WriteLine("yes....yes it did");
                        SpecResponseObject PressXtoJSON = new SpecResponseObject();
                        try
                        {
                            PressXtoJSON = await httpResponse.Content.ReadAsAsync<SpecResponseObject>();
                            if (PressXtoJSON.Error)
                            {
                                Console.WriteLine("the faiL, has arrived :(");
                                foreach (Content_Centre_API_CORS.ErrorHandling.SpecificationError m in PressXtoJSON.ErrorList)
                                {
                                    Console.WriteLine("-> " + m.ErrorMessage);
                                }
                            }
                            else
                            {
                                Console.WriteLine("SpecCheck was a GREAT SUCCESS");
                                DataTable tempResponse = new DataTable();
                                tempResponse = JsonConvert.DeserializeObject<DataTable>(PressXtoJSON.JsonResults);
                                try{
                                    newSpec.SpecID = Convert.ToInt32(tempResponse.Rows[0].ItemArray[0]);
                                }
                                
                                    catch(SystemException e){
                                        Console.WriteLine(e.Message.ToString());
                                    }
                                if (newSpec.SpecID < 0)
                                {
                                    Console.WriteLine("no, no, no.... it's not there... can't update");
                                    newSpec.SpecID = 0;
                                }
                                // check if there are matching spec and assign the new spec ID
                            }
                        }
                        catch (Newtonsoft.Json.JsonSerializationException e)
                        {
                            Console.WriteLine("the faiL, has arrived :(");
                            Console.WriteLine(e.Message);
                        }
                        //SpecRes = JsonConvert.DeserializeObject<List<SpecTransferObject>>(PressXtoJSON.JsonResults);

                    }
                    else
                    {
                        Console.WriteLine("nope, wrong again!");
                        Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
                    }
                }

                if (temp.tempSpecID < 0) // INSERT
                {

                    httpResponse = client.PostAsJsonAsync<SpecTransferObject>(ajaxURI + "/insertSpec", newSpec).Result;
                    Console.WriteLine("\n|--------------------|");
                    Console.WriteLine("... Getting Spec ID for new Spec");
                    Console.WriteLine("Data Sent.. did it work? ....");

                    if (httpResponse.StatusCode == (HttpStatusCode)200)
                    {
                        Console.WriteLine("yes....yes it did");
                        SpecResponseObject PressXtoJSON = new SpecResponseObject();
                        try
                        {
                            PressXtoJSON = await httpResponse.Content.ReadAsAsync<SpecResponseObject>();
                            if (PressXtoJSON.Error)
                            {
                                Console.WriteLine("the faiL, has arrived :(");
                                foreach (Content_Centre_API_CORS.ErrorHandling.SpecificationError m in PressXtoJSON.ErrorList)
                                {
                                    Console.WriteLine("-> " + m.ErrorMessage);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Spec was a GREAT SUCCESS");
                                newSpec.SpecID = PressXtoJSON.SpecID;
                                int toChange = test.menuDataList[xList].tempSpecID;
                                test.menuDataList[xList].tempSpecID = PressXtoJSON.SpecID;
                                // check if there are matching spec and assign the new spec ID

                                

                                for (int xUpdate = xList + 1; xUpdate < howMany + start; xUpdate++)
                                {
                                    if (test.menuDataList[xUpdate].tempSpecID == toChange)
                                        test.menuDataList[xUpdate].tempSpecID = newSpec.SpecID;
                                }
                            }
                        }
                        catch (Newtonsoft.Json.JsonSerializationException e)
                        {
                            Console.WriteLine("the faiL, has arrived :(");
                            Console.WriteLine(e.Message);
                        }
                        //SpecRes = JsonConvert.DeserializeObject<List<SpecTransferObject>>(PressXtoJSON.JsonResults);

                    }
                    else
                    {
                        Console.WriteLine("nope, wrong again!");
                        Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
                    }
                }
                else if (temp.tempSpecID != 0)
                { // UPDATE!!!!
                    httpResponse = client.PostAsJsonAsync<SpecTransferObject>(ajaxURI + "/updateSpec", newSpec).Result;
                    Console.WriteLine("\n|--------------------|");
                    Console.WriteLine("... Getting Spec ID for new Spec");
                    Console.WriteLine("Data Sent.. did it work? ....");

                    if (httpResponse.StatusCode == (HttpStatusCode)200)
                    {
                        Console.WriteLine("yes....yes it did");
                        SpecResponseObject PressXtoJSON = new SpecResponseObject();
                        try
                        {
                            PressXtoJSON = await httpResponse.Content.ReadAsAsync<SpecResponseObject>();
                            if (PressXtoJSON.Error)
                            {
                                Console.WriteLine("the faiL, has arrived :(");
                                foreach (Content_Centre_API_CORS.ErrorHandling.SpecificationError m in PressXtoJSON.ErrorList)
                                {
                                    Console.WriteLine("-> " + m.ErrorMessage);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Spec was a GREAT SUCCESS");
                            }
                        }
                        catch (Newtonsoft.Json.JsonSerializationException e)
                        {
                            Console.WriteLine("the faiL, has arrived :(");
                            Console.WriteLine(e.Message);
                        }
                        //SpecRes = JsonConvert.DeserializeObject<List<SpecTransferObject>>(PressXtoJSON.JsonResults);

                    }
                    else
                    {
                        Console.WriteLine("nope, wrong again!");
                        Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
                    }
                }
                else
                {
                    Console.WriteLine("Spec ID is invalid, i don't know what to do");
                }



                List<SpecificationItemTransferObject> courses = new List<SpecificationItemTransferObject>();

                int headerCounter = 1, subItemCounter = 1;

                for (int x = 0; x < temp.menusList[0].coursesList.Count; x++)
                {
                    COURSE m = temp.menusList[0].coursesList[x];
                    SpecificationItemTransferObject tempCourse = new SpecificationItemTransferObject();
                    tempCourse.AccountID = "1"; // Air France
                    int xID = x + 1;

                    tempCourse.SpecificationItemID = -(courses.Count + 1);
                    tempCourse.LanguageID = getLanguageID(Languages, temp.language);
                    tempCourse.SpecificationID = newSpec.SpecID;

                    // get Allergen IDs and apply to specItem
                    foreach (ALLERGEN m2 in m.allergens)
                    {
                        AllergenTransferObject tempAlly = new AllergenTransferObject();

                        tempAlly.AccountID = Convert.ToInt32(tempCourse.AccountID);
                        tempAlly.LanguageID = tempCourse.LanguageID;
                        tempAlly.AllergenID = getAllergenID(Allergens, m2.allergen, tempAlly.LanguageID);
                        tempCourse.Allergens.Add(tempAlly);
                    }



                    if (m.label != null)
                    {
                        tempCourse.ParentID = 0; // this is a header
                        tempCourse.Value = m.label;
                        tempCourse.OrderNumber = headerCounter++;
                        subItemCounter = 1;
                    }
                    else
                    {
                        if (x > 0)
                        {
                            int parentX = 0;
                            while (courses[(courses.Count - 1) - parentX].ParentID != 0)
                            {
                                parentX++;
                            }
                            tempCourse.ParentID = courses[(courses.Count - 1) - parentX].SpecificationItemID;
                            tempCourse.Value = m.content;
                            tempCourse.OrderNumber = subItemCounter++;
                        }
                    }



                    courses.Add(tempCourse);

                    // Check for children
                    if (m.label != null)
                    {
                        if (m.content != "") // feed the children
                        {
                            SpecificationItemTransferObject courseRei = new SpecificationItemTransferObject();
                            //xID = x + 2;
                            courseRei.SpecificationItemID = -(courses.Count + 1);
                            courseRei.LanguageID = getLanguageID(Languages, temp.language);
                            courseRei.SpecificationID = newSpec.SpecID;
                            courseRei.ParentID = courses[courses.Count - 1].SpecificationItemID;
                            courseRei.Value = m.content;
                            courseRei.OrderNumber = subItemCounter++;

                            // move allergens from the header
                            courseRei.Allergens = tempCourse.Allergens;
                            tempCourse.Allergens = new List<AllergenTransferObject>();
                            courses.Add(courseRei);
                        }
                    }
                }



                str = "\n";
                str += newSpec.Code + " : "
                    + newSpec.Class + " : "
                    + newSpec.RotationID + " : "
                    + newSpec.MealTypeID + " : " 
                    + newSpec.StatusID + " : "
                    + newSpec.Station;

                Console.WriteLine(str);


                foreach (SpecificationItemTransferObject m in courses)
                {
                    str = "";
                    str += m.OrderNumber + ":"
                        + m.ParentID + ":"
                        + m.SpecificationID + ":"
                        + m.SpecificationItemID + ":  ";
                    if (m.ParentID != 0)
                        str += " ";
                    str += m.Value;
                    Console.WriteLine(str);
                }
                Console.WriteLine("enter?");
                Console.ReadLine();



                if (newSpec.SpecID > 0)
                {
                    // ----------- Process the Spec Items .......
                    SpecificationItemTransferObjects SpecItemDTO = new SpecificationItemTransferObjects();
                    SpecificationItemTransferObject tempSpecGet = new SpecificationItemTransferObject();
                    //SpecItemDTO.data = courses;

                    //SpecItemDTO

                    tempSpecGet.SpecificationID = newSpec.SpecID;
                    //SpecItemDTO.data[0].SpecificationID = newSpec.SpecID;

                    //DataTable tempResponse = new DataTable();
                    //SpecificationItemResponseObject SpecTemp = new SpecificationItemResponseObject();
                    //SpecTemp = await httpResponse.Content.ReadAsAsync<SpecificationItemResponseObject>();

                    //SpecItemDTO = JsonConvert.DeserializeObject<SpecificationItemTransferObjects>(SpecTemp.JsonResults);
                    // nuke old all specItems
                    // will take longer but checking, updating and deleting would be better, but slower for

                    //get old Specs
                    httpResponse = client.PostAsJsonAsync<SpecificationItemTransferObject>(ajaxURI + "/getSpecificationItemsBySpecID", tempSpecGet).Result;
                    SpecificationItemResponseObject toJSON = new SpecificationItemResponseObject();
                    List<SpecificationItemTransferObject> CorSair = new List<SpecificationItemTransferObject>();
                    toJSON = await httpResponse.Content.ReadAsAsync<SpecificationItemResponseObject>();
                    CorSair = JsonConvert.DeserializeObject<List<SpecificationItemTransferObject>>(toJSON.JsonResults);

                    
                    // mark all as toDelete
                    foreach (SpecificationItemTransferObject m in CorSair)
                    {
                        if (m.LanguageID == courses[0].LanguageID)
                            m.ToBeDeleted = true;
                    }


                    // append to courses
                    courses.AddRange(CorSair);
                    SpecItemDTO.data = courses;



                    httpResponse = client.PostAsJsonAsync<SpecificationItemTransferObjects>(ajaxURI + "/ProcessSpecificationItems", SpecItemDTO).Result;
                    Console.WriteLine("\n|--------------------|");
                    Console.WriteLine("... Attaching SpecItems for new Spec");
                    Console.WriteLine("Data Sent.. did it work? ....");

                    if (httpResponse.StatusCode == (HttpStatusCode)200)
                    {
                        Console.WriteLine("yes....yes it did");
                        SpecificationItemResponseObject PressXtoJSON = new SpecificationItemResponseObject();
                        try
                        {
                            PressXtoJSON = await httpResponse.Content.ReadAsAsync<SpecificationItemResponseObject>();
                            //if (PressXtoJSON.Error)
                            //{
                            //    Console.WriteLine("the faiL, has arrived :(");
                            //    foreach (Content_Centre_API_CORS.ErrorHandling.SpecificationError m in PressXtoJSON.ErrorList)
                            //    {
                            //        Console.WriteLine("-> " + m.ErrorMessage);
                            //    }
                            //}
                            //else
                            //{
                            //    Console.WriteLine("Spec was a GREAT SUCCESS");
                            //    newSpec.SpecID = PressXtoJSON.SpecID;
                            //}
                        }
                        catch (Newtonsoft.Json.JsonSerializationException e)
                        {
                            Console.WriteLine("the faiL, has arrived :(");
                            Console.WriteLine("-> " + e.Message);
                        }
                        //SpecRes = JsonConvert.DeserializeObject<List<SpecTransferObject>>(PressXtoJSON.JsonResults);

                    }
                    else
                    {
                        Console.WriteLine("nope, wrong again!");
                        Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
                    }
                }
                else
                {
                    Console.WriteLine("can't add specs without ID");
                }
                Console.Clear();
            }
            Console.WriteLine("one more ENTER");
            Console.ReadLine();


        }

        public int getAllergenID(List<AllergenTransferObject> Allergens, String finder, int LanguageID)
        {
            AllergenTransferObject findThis = new AllergenTransferObject();
            finder = finder.ToLower();
            findThis.Allergen = finder;
            findThis.LanguageID = LanguageID;

            for (int m = 0; m < Allergens.Count; m++)
            {
                if (Allergens[m].Allergen.ToLower() == findThis.Allergen && Allergens[m].LanguageID == findThis.LanguageID)
                {
                    findThis.AllergenID = Allergens[m].AllergenID;
                }
            }
            return findThis.AllergenID;
        }

        public int getLanguageID(List<LanguageTransferObject> Languages, String finder)
        {
            LanguageTransferObject findThis = new LanguageTransferObject();
            finder = finder.ToLower();
            findThis.LanguageCode = finder;

            for (int m = 0; m < Languages.Count; m++)
            {
                if (Languages[m].LanguageCode.ToLower() == findThis.LanguageCode)
                {
                    findThis.LanguageID = Languages[m].LanguageID;
                }
            }
            return findThis.LanguageID;
        }

        public int getMealTypeID(List<MealTypeTransferObject> MealTypes, String finder)
        {
            MealTypeTransferObject findThis = new MealTypeTransferObject();
            finder = finder.ToLower();
            findThis.MealType = finder; //"CSML";

            for (int m = 0; m < MealTypes.Count; m++)
            {
                if (MealTypes[m].MealType.ToLower() == findThis.MealType)
                {
                    findThis.MealTypeID = MealTypes[m].MealTypeID;
                }
            }
            return findThis.MealTypeID;
        }

        public int getRotationGrillID(List<RotationGrillTransferObject> Rotations, String finder)
        {
            RotationGrillTransferObject findThis = new RotationGrillTransferObject();
            finder = finder.ToLower();
            findThis.RotationName = finder;

            for (int m = 0; m < Rotations.Count; m++)
            {
                if (Rotations[m].RotationName.ToLower() == findThis.RotationName)
                {
                    findThis.RotationID = Rotations[m].RotationID;
                }
            }
            return findThis.RotationID;
        }

        public int getStatusID(List<StatusTransferObject> Statuses, String finder)
        {
            StatusTransferObject findThis = new StatusTransferObject();
            finder = finder.ToLower();
            if (finder == "")
                finder = "in progress";
            if (finder == "brouillon") // draft?
                finder = "in progress";

            findThis.Status = finder;

            for (int m = 0; m < Statuses.Count; m++)
            {
                if (Statuses[m].Status.ToLower() == findThis.Status)
                {
                    findThis.StatusID = Statuses[m].StatusID;
                }
            }
            return findThis.StatusID;
        }

        protected override void OnStart(string[] args)
        {


        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

        public async Task<String> GetAuthToken()
        {

            String accessToken = "";

            try
            {
                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = cookies;

                HttpClient client = new HttpClient(handler);
                Uri uri = new Uri("https://oscid.osc-it.com/account/signin");
                HttpResponseMessage response = client.GetAsync(uri).Result;

                string content = await response.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(content);
                string tokenValue = "";
                var reqVerTokenElement = htmlDoc
                                    .DocumentNode
                                    .Descendants("input")
                                    .Where(n => n.Attributes["name"] != null
                                                && n.Attributes["name"].Value
                                                    == "__RequestVerificationToken")
                                    .FirstOrDefault();

                if (reqVerTokenElement != null)
                {
                    tokenValue = reqVerTokenElement.Attributes["value"].Value;
                }

                string postUrl = "https://oscid.osc-it.com/account/signin";

                StringContent postContent = new StringContent("__RequestVerificationToken=" + tokenValue + "&UserName=frankAF&Password=asdqwe123&EnableSSO=false");
                postContent.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
                HttpResponseMessage newResponse = client.PostAsync(new Uri(postUrl), postContent).Result;

                //String jwtURL = "https://oscid.osc-it.com/issue/oauth2/authorize?client_id=tt_ContentCentreProdInternal&scope=urn%3AContentCentreProdInternal&response_type=token&redirect_uri=https%3A%2F%2Fcontentcentre.oscww.com%2F";
                String jwtURL = "https://oscid.osc-it.com/issue/oauth2/authorize?client_id=tt_ContentCentreWeb&scope=urn%3AContentCentreWeb&response_type=token&redirect_uri=https%3A%2F%2Flocalhost%2FContentCentreWeb%2Findex.html";
                response = client.GetAsync(jwtURL).Result;
                accessToken = response.RequestMessage.RequestUri.Fragment.Substring(0, response.RequestMessage.RequestUri.Fragment.IndexOf("&"));

            }
            catch (Exception ex)
            {
            }

            return accessToken;

        }
    }
}
