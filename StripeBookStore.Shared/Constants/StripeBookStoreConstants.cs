using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StripeBookStore.Shared.Interfaces;
using StripeBookStore.Shared.Models;

namespace StripeBookStore.Shared.Constants
{
    public static class StripeBookStoreConstants
    {
        public const string StripeBookStoreBaseUrl = "http://localhost:42424/";

        public static readonly ObservableCollection<Book> BooksCollection = new ObservableCollection<Book>()
        {
            new Book()
            {
                Sku = "book_3x12",
                Name = "Harry Potter and the Sorcerer's Stone",
                Price = 999,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque accumsan urna a nisi elementum, id vulputate tortor fringilla.",
                Genre = "Fantasy Fiction"
            },
            new Book()
            {
                Sku = "book_2x332",
                Name = "Don Quixote",
                Price = 199,
                Description = "Sed vitae eleifend nisi. Etiam metus odio, dapibus luctus dapibus non, porta fermentum velit.",
                Genre = "Parody"
            },
            new Book()
            {
                Sku = "book_7x142",
                Name = "The Little Prince",
                Price = 1099,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque accumsan urna a nisi elementum, id vulputate tortor fringilla.",
                Genre = "Children's Literature"
            },
            new Book()
            {
                Sku = "book_11x77",
                Name = "The Great Gatsby",
                Price = 1199,
                Description = "Sed vitae eleifend nisi. Etiam metus odio, dapibus luctus dapibus non, porta fermentum velit.",
                Genre = "Novel"
            },
            new Book()
            {
                Sku = "book_3x12",
                Name = "Moby Dick",
                Price = 379,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque accumsan urna a nisi elementum, id vulputate tortor fringilla.",
                Genre = "Nautical Fiction"
            },
            new Book()
            {
                Sku = "book_3x12",
                Name = "Harry Potter and the Sorcerer's Stone",
                Price = 999,
                Description = "Sed vitae eleifend nisi. Etiam metus odio, dapibus luctus dapibus non, porta fermentum velit.",
                Genre = "Fantasy Fiction"
            },
            new Book()
            {
                Sku = "book_2x332",
                Name = "Don Quixote",
                Price = 199,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque accumsan urna a nisi elementum, id vulputate tortor fringilla.",
                Genre = "Parody"
            },
            new Book()
            {
                Sku = "book_7x142",
                Name = "The Little Prince",
                Price = 1099,
                Description = "Sed vitae eleifend nisi. Etiam metus odio, dapibus luctus dapibus non, porta fermentum velit.",
                Genre = "Children's Literature"
            },
            new Book()
            {
                Sku = "book_11x77",
                Name = "The Great Gatsby",
                Price = 1199,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque accumsan urna a nisi elementum, id vulputate tortor fringilla.",
                Genre = "Novel"
            },
            new Book()
            {
                Sku = "book_3x12",
                Name = "Moby Dick",
                Price = 379,
                Description = "Sed vitae eleifend nisi. Etiam metus odio, dapibus luctus dapibus non, porta fermentum velit.",
                Genre = "Nautical Fiction"
            },
        };
    }
        
}
