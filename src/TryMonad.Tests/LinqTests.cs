using System;
using System.Linq;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

// http://community.bartdesmet.net/blogs/bart/archive/2008/08/19/probably-the-most-powerful-linq-operator-selectmany.aspx

namespace TryMonad.Tests {

    public class Product {
        public string ProductName { set; get; }
    }

    public class Shop {
        public string Name { set; get; }
        public IEnumerable<Product> Products { set; get; } = new List<Product>();
    }

    public class LinqTests {
        private IEnumerable<Shop> CreateShops() {
            var shop = new Shop {
                Name = "Shop A",
                Products = new[] {
                    new Product { ProductName = "Product A" },
                    new Product { ProductName = "Product B" },
                    new Product { ProductName = "Product C" },
                }
            };

            var shops = new List<Shop>();
            shops.Add(shop);
            shops.Add(shop);
            return shops;
        }

        private IEnumerable<Product> GetProducts(IEnumerable<Shop> shops) {
            foreach (var shop in shops) {
                foreach (var products in shop.Products) {
                    yield return products;
                }
            }
        }

        private IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector) {
            foreach (var item in source) {
                foreach (var collection in collectionSelector(item)) {
                    yield return resultSelector(item, collection);
                }
            }
        }

        [Fact]
        public void LoopFlatTest() {
            var shops = CreateShops();
            var products = GetProducts(shops);
            products.Count().Should().Be(6);
        }

        [Fact]
        public void SelectManyTest() {
            var shops = CreateShops();
            var result = SelectMany<Shop, Product, String>(shops, x => x.Products, (x, y) => x.Name + y.ProductName);
            result.Count().Should().Be(6);
        }

        [Fact]
        public void FlatProductsTest() {
            var shops = CreateShops();

            var products = shops.SelectMany(x => x.Products, (s, p) => s.Name + ":" + p.ProductName);
            products.Count().Should().Be(6);
        }

        [Fact]
        public void CartesianProductTest() {
            var a = new[] { 1, 4, 7 };
            var b = new[] { 2, 5, 8 };

            var result =
                from aa in a
                from bb in b
                select aa + bb;

            result.Count().Should().Be(9);

            var lampda = a.SelectMany(aa => b, (aa, bb) => new { aa, bb });
            lampda.Count().Should().Be(9);
        }
    }
}

