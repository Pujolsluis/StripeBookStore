﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StripeBookStore.Shared.Interfaces;
using StripeBookStore.Shared.Models;

namespace StripeBookStore.Shared.Constants
{
    public static class StripeBookStoreConstants
    {
        public const string StripeBookStoreBaseUrl = "http://localhost:42424/api";
        //Android localhost
        //public const string StripeBookStoreBaseUrl = "http://10.0.2.2:42424/api";

        public static readonly ObservableCollection<Book> BooksCollection = new ObservableCollection<Book>()
        {
            new Book()
            {
                Sku = "book_3x12",
                Name = "Harry Potter And The Sorcerer's Stone",
                Price = 999,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque accumsan urna a nisi elementum, id vulputate tortor fringilla.",
                Genre = "Fantasy Fiction",
                ImageUri = "https://m.media-amazon.com/images/I/51U4p-ir2BL._SL500_.jpg"
            },
            new Book()
            {
                Sku = "book_2x332",
                Name = "Don Quixote",
                Price = 199,
                Description = "Sed vitae eleifend nisi. Etiam metus odio, dapibus luctus dapibus non, porta fermentum velit.",
                Genre = "Parody",
                ImageUri = "https://prodimage.images-bn.com/pimages/9780062391667_p0_v1_s550x406.jpg"
            },
            new Book()
            {
                Sku = "book_7x142",
                Name = "The Little Prince",
                Price = 1099,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque accumsan urna a nisi elementum, id vulputate tortor fringilla.",
                Genre = "Children's Literature",
                ImageUri = "https://upload.wikimedia.org/wikipedia/en/0/05/Littleprince.JPG"
            },
            new Book()
            {
                Sku = "book_11x77",
                Name = "The Great Gatsby",
                Price = 1199,
                Description = "Sed vitae eleifend nisi. Etiam metus odio, dapibus luctus dapibus non, porta fermentum velit.",
                Genre = "Novel",
                ImageUri = "https://images.fineartamerica.com/images-medium-large-5/the-great-gatsby-book-cover-movie-poster-art-2-nishanth-gopinathan.jpg"
            },
            new Book()
            {
                Sku = "book_3x12",
                Name = "Moby Dick",
                Price = 379,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque accumsan urna a nisi elementum, id vulputate tortor fringilla.",
                Genre = "Nautical Fiction",
                ImageUri = "https://images-na.ssl-images-amazon.com/images/I/51lGqAiGmvL._SX332_BO1,204,203,200_.jpg"
            },
            new Book()
            {
                Sku = "book_3x12",
                Name = "Harry Potter And The Sorcerer's Stone",
                Price = 999,
                Description = "Sed vitae eleifend nisi. Etiam metus odio, dapibus luctus dapibus non, porta fermentum velit.",
                Genre = "Fantasy Fiction",
                ImageUri = "https://m.media-amazon.com/images/I/51U4p-ir2BL._SL500_.jpg"
            },
            new Book()
            {
                Sku = "book_2x332",
                Name = "Don Quixote",
                Price = 199,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque accumsan urna a nisi elementum, id vulputate tortor fringilla.",
                Genre = "Parody",
                ImageUri = "https://prodimage.images-bn.com/pimages/9780062391667_p0_v1_s550x406.jpg"
            },
            new Book()
            {
                Sku = "book_7x142",
                Name = "The Little Prince",
                Price = 1099,
                Description = "Sed vitae eleifend nisi. Etiam metus odio, dapibus luctus dapibus non, porta fermentum velit.",
                Genre = "Children's Literature",
                ImageUri = "https://upload.wikimedia.org/wikipedia/en/0/05/Littleprince.JPG"
            },
            new Book()
            {
                Sku = "book_11x77",
                Name = "The Great Gatsby",
                Price = 1199,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque accumsan urna a nisi elementum, id vulputate tortor fringilla.",
                Genre = "Novel",
                ImageUri = "https://images.fineartamerica.com/images-medium-large-5/the-great-gatsby-book-cover-movie-poster-art-2-nishanth-gopinathan.jpg"
            },
            new Book()
            {
                Sku = "book_3x12",
                Name = "Moby Dick",
                Price = 379,
                Description = "Sed vitae eleifend nisi. Etiam metus odio, dapibus luctus dapibus non, porta fermentum velit.",
                Genre = "Nautical Fiction",
                ImageUri = "https://images-na.ssl-images-amazon.com/images/I/51lGqAiGmvL._SX332_BO1,204,203,200_.jpg"
            },
        };
    }
        
}
