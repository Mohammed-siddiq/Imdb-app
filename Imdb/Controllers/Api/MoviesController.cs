using Imdb.Models;
using Imdb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Imdb.Controllers.Api
{
    public class MoviesController : ApiController
    {
        [Route("api/Movies/GetMovieDetails/{MovieName}")]
        [HttpGet]
        public dynamic GetMovieDetails(string MovieName)
        {
            string baseurl = "https://api.themoviedb.org/3/";
            string apikey = "api_key=3fae25f1c2fc7c75b43ef7583be8adbf&language=en-US";
            string query = "search/movie";
            string search = "&query=" + MovieName;
            
            dynamic json = Request_api(baseurl, query, apikey,search);
            var enumerableResults = (dynamic)null; 
            try
            {
                 enumerableResults = (json["results"] as IEnumerable<dynamic>).ToList();

            }
            catch (Exception e)
            {
                return null;

            }
            if (enumerableResults == null || enumerableResults.Count == 0)
            {
                return null;
            }
            var NewMovie = new MovieViewModel();
            var result = enumerableResults[0];
            string overview = result["overview"];
            string releasedate = result["release_date"];

            NewMovie.Poster = "http://image.tmdb.org/t/p/w185/" + result["poster_path"];
            NewMovie.Name = result["title"];
            NewMovie.Plot = overview;
            NewMovie.YearOfRealease = int.Parse(releasedate.Substring(0, 4));
            int id = result["id"];
            query = "movie/" + id + "/credits";
            search = "";
            json = Request_api(baseurl, query, apikey, search);
            var Casts = (json["cast"] as IEnumerable<dynamic>).ToList();
            var Crew = (json["crew"] as IEnumerable<dynamic>).ToList();
            for(int i=0;i<Casts.Count;i++)
            {
                if (i == 3)
                    break;
                Person p = new Person();
                var cast = Casts[i];
                string name = cast["name"];
                int cid = cast["id"];
                query = "person/" + cid;
                search = "";
                json = Request_api(baseurl, query, apikey, search);
                p = Makeperson(json);
               
                NewMovie.Actors.Add(p);

            }

            foreach (var c in Crew)
            {
                string name = c["name"];
                string role = c["job"];
                if (role.Equals("Producer"))
                {
                    Person p = new Person();
                    int cid = c["id"];
                    query = "person/" + cid;
                    search = "";
                    json = Request_api(baseurl, query, apikey, search);
                    p = Makeperson(json);
                    NewMovie.Producer = p;
                    break;
                }


            }

            var jsonResult = new JavaScriptSerializer().Serialize(NewMovie);

            return jsonResult;


        }

        private Person Makeperson(dynamic json)
        {
            Person p = new Person();

            p.Name = json["name"];
            if (json["biography"] != null)
                p.Bio = json["biography"];
            else
                p.Bio = "Bio Not Available!";
            if (json["birthday"] != null && json["birthday"].Length != 0)
            {
                string date = json["birthday"];
                try
                {
                    string[] Adate = date.Split('-');
                    p.Dob = new DateTime(int.Parse(Adate[0]), int.Parse(Adate[1]), int.Parse(Adate[2]));

                }
                catch (Exception e)
                {
                    p.Dob = DateTime.Parse("1/1/1");
                }
                p.Sex = (json["gender"] == 1) ? "Female" : "Male";

            }
            return p;

        }

        private object Request_api(string baseurl, string query, string apikey, string search)
        {
            string url = baseurl + query + "?" + apikey + search;
            var httpclient = new HttpClient();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = httpclient.SendAsync(request).Result;
                var serializer = new JavaScriptSerializer();
                dynamic json = serializer.Deserialize<object>(response.Content.ReadAsStringAsync().Result);
                return json;

            }
            catch (Exception e)
            {
                return null;

            }
            



        }
    }
}
