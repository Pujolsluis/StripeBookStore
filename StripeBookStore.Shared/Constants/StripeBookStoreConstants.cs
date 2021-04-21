using System;
using System.Collections.Generic;
using StripeBookStore.Shared.Models;

namespace StripeBookStore.Shared.Constants
{
    public static class StripeBookStoreConstants
    {
        public const string StripeBookStoreBaseUrl = "http://localhost:42424/";

        public static readonly List<Book> BooksCollection = new List<Book>()
        {
            new Book()
            {
                Sku = "book_3x12",
                Name = "Harry Potter and the Sorcerer's Stone",
                Price = 999,
                Genre = "Fantasy Fiction"
            },
            new Book()
            {
                Sku = "book_2x332",
                Name = "Don Quixote",
                Price = 199,
                Genre = "Parody"
            },
            new Book()
            {
                Sku = "book_7x142",
                Name = "The Little Prince",
                Price = 1099,
                Genre = "Children's Literature"
            },
            new Book()
            {
                Sku = "book_11x77",
                Name = "The Great Gatsby",
                Price = 1199,
                Genre = "Novel"
            },
            new Book()
            {
                Sku = "book_3x12",
                Name = "Moby Dick",
                Price = 379,
                Genre = "Nautical Fiction"
            },
        };
    }
        
}
