﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI.Static
{
    public static class Endpoints
    {
        // Change the BaseUrl to match the URL location of the API
        public static string BaseUrl = "https://localhost:44355/";
        public static string AuthorsEndpoint = $"{BaseUrl}api/authors/";
        public static string BooksEndpoint = $"{BaseUrl}api/books/";
        public static string RegisterEndpoint = $"{BaseUrl}api/users/register/";
        public static string LoginEndpoint = $"{BaseUrl}api/users/login/";

    }
}
