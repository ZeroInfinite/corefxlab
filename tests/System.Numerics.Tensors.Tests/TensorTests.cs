// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Numerics;
using Xunit;

namespace tests
{
    public class TensorTests
    {
        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void ConstructTensorFromArrayRank1(bool columnMajor)
        {
            // use array to avoid calling params int[] overload
            Array array = new[] { 0, 1, 2 };
            var tensor = new DenseTensor<int>(array, columnMajor);

            // single dimensional tensors are always row and column major
            Assert.Equal(true, tensor.IsReveresedStride);
            Assert.Equal(0, tensor[0]);
            Assert.Equal(1, tensor[1]);
            Assert.Equal(2, tensor[2]);
        }

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void ConstructTensorFromArrayRank2(bool columnMajor)
        {
            var tensor = new DenseTensor<int>(new[,]
            {
                {0, 1, 2},
                {3, 4, 5}
            }, columnMajor);

            Assert.Equal(columnMajor, tensor.IsReveresedStride);
            Assert.Equal(0, tensor[0, 0]);
            Assert.Equal(1, tensor[0, 1]);
            Assert.Equal(2, tensor[0, 2]);
            Assert.Equal(3, tensor[1, 0]);
            Assert.Equal(4, tensor[1, 1]);
            Assert.Equal(5, tensor[1, 2]);
        }

        /*
        [Fact]
        public void TestDefaultConstructor()
        {
            Tensor<int> defaultTensor = default(Tensor<int>);

            Assert.Equal(0, defaultTensor.Buffer.Length);
            Assert.Equal(0, defaultTensor.Dimensions.Count);
            Assert.True(defaultTensor.IsFixedSize);
            Assert.False(defaultTensor.IsReadOnly);
            Assert.Equal(0, defaultTensor.Length);
            Assert.Equal(0, defaultTensor.Rank);
            Assert.False(defaultTensor.IsReveresedStride);

            // don't throw
            var clone = defaultTensor.Clone();
            clone = defaultTensor.CloneEmpty();
            var doubleClone = defaultTensor.CloneEmpty<double>();
            var arr = new int[0];
            defaultTensor.CopyTo(arr, 0);
            defaultTensor.Fill(0);
            Assert.Throws<ArgumentException>("dimensions", () => defaultTensor.Reshape());
            Assert.Throws<ArgumentOutOfRangeException>("dimensions", () => defaultTensor.Reshape(0));
            Assert.Throws<ArgumentException>("dimensions", () => defaultTensor.ReshapeCopy());
            Assert.Throws<ArgumentOutOfRangeException>("dimensions", () => defaultTensor.ReshapeCopy(0));
            defaultTensor.ToString();

            Assert.Equal(default(Tensor<int>), defaultTensor);
            Assert.Equal(default(Tensor<int>).GetHashCode(), defaultTensor.GetHashCode());
            Assert.False(defaultTensor.GetEnumerator().MoveNext());

            // rank is too small
            Assert.Throws<InvalidOperationException>(() => defaultTensor.GetDiagonal());
            Assert.Throws<InvalidOperationException>(() => defaultTensor.GetTriangle());
            Assert.Throws<InvalidOperationException>(() => defaultTensor.GetUpperTriangle());

            Assert.Throws<ArgumentOutOfRangeException>(() => defaultTensor[0]);
            Assert.Throws<ArgumentOutOfRangeException>(() => defaultTensor[0, 0]);
            Assert.Throws<ArgumentOutOfRangeException>(() => defaultTensor[0, 0, 0]);
            
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor + 1);
            Assert.Throws<ArgumentException>("left", () => defaultTensor + defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => +defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor - 1);
            Assert.Throws<ArgumentException>("left", () => defaultTensor - defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => -defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor++);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor--);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor * 1);
            Assert.Throws<ArgumentException>("left", () => defaultTensor * defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor / 1);
            Assert.Throws<ArgumentException>("left", () => defaultTensor / defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor % 1);
            Assert.Throws<ArgumentException>("left", () => defaultTensor % defaultTensor);
            Assert.Throws<ArgumentException>("left", () => defaultTensor & defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor & -1);
            Assert.Throws<ArgumentException>("left", () => defaultTensor | defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor | -1);
            Assert.Throws<ArgumentException>("left", () => defaultTensor ^ defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor ^ -1);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor >> 1);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor << 1);
        }
        */

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void ConstructTensorFromArrayRank3(bool columnMajor)
        {
            var arr = new[, ,]
            {
                {
                    {0, 1, 2},
                    {3, 4, 5}
                },
                {
                    {6, 7 ,8 },
                    {9, 10 ,11 },
                },
                {
                    {12, 13 ,14 },
                    {15, 16 ,17 },
                },
                {
                    {18, 19 ,20 },
                    {21, 22 ,23 },
                }
            };
            var tensor = new DenseTensor<int>(arr, columnMajor);

            Assert.Equal(columnMajor, tensor.IsReveresedStride);

            Assert.Equal(0, tensor[0, 0, 0]);
            Assert.Equal(1, tensor[0, 0, 1]);
            Assert.Equal(2, tensor[0, 0, 2]);
            Assert.Equal(3, tensor[0, 1, 0]);
            Assert.Equal(4, tensor[0, 1, 1]);
            Assert.Equal(5, tensor[0, 1, 2]);

            Assert.Equal(6, tensor[1, 0, 0]);
            Assert.Equal(7, tensor[1, 0, 1]);
            Assert.Equal(8, tensor[1, 0, 2]);
            Assert.Equal(9, tensor[1, 1, 0]);
            Assert.Equal(10, tensor[1, 1, 1]);
            Assert.Equal(11, tensor[1, 1, 2]);

            Assert.Equal(12, tensor[2, 0, 0]);
            Assert.Equal(13, tensor[2, 0, 1]);
            Assert.Equal(14, tensor[2, 0, 2]);
            Assert.Equal(15, tensor[2, 1, 0]);
            Assert.Equal(16, tensor[2, 1, 1]);
            Assert.Equal(17, tensor[2, 1, 2]);

            Assert.Equal(18, tensor[3, 0, 0]);
            Assert.Equal(19, tensor[3, 0, 1]);
            Assert.Equal(20, tensor[3, 0, 2]);
            Assert.Equal(21, tensor[3, 1, 0]);
            Assert.Equal(22, tensor[3, 1, 1]);
            Assert.Equal(23, tensor[3, 1, 2]);
        }

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void ConstructFromDimensions(bool columnMajor)
        {
            var tensor = new DenseTensor<int>(columnMajor, 1, 2, 3);
            Assert.Equal(3, tensor.Rank);
            Assert.Equal(3, tensor.Dimensions.Count);
            Assert.Equal(1, tensor.Dimensions[0]);
            Assert.Equal(2, tensor.Dimensions[1]);
            Assert.Equal(3, tensor.Dimensions[2]);
            Assert.Equal(6, tensor.Length);

            Assert.Throws<ArgumentNullException>("dimensions", () => new DenseTensor<int>(dimensions: null));
            Assert.Throws<ArgumentException>("dimensions", () => new DenseTensor<int>(dimensions: new int[0]));

            Assert.Throws<ArgumentOutOfRangeException>("dimensions", () => new DenseTensor<int>(dimensions: new[] { 1, 0 }));
            Assert.Throws<ArgumentOutOfRangeException>("dimensions", () => new DenseTensor<int>(dimensions: new[] { 1, -1 }));

            // ensure dimensions are immutable
            var dimensions = new[] { 1, 2, 3 };
            tensor = new DenseTensor<int>(dimensions:dimensions);
            dimensions[0] = dimensions[1] = dimensions[2] = 0;
            Assert.Equal(1, tensor.Dimensions[0]);
            Assert.Equal(2, tensor.Dimensions[1]);
            Assert.Equal(3, tensor.Dimensions[2]);
        }

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void ConstructTensorFromArrayRank3WithLowerBounds(bool columnMajor)
        {
            var dimensions = new[] { 2, 3, 4 };
            var lowerBounds = new[] { 0, 5, 200 };
            var arrayWithLowerBounds = Array.CreateInstance(typeof(int), dimensions, lowerBounds);

            int value = 0;
            for(int x = lowerBounds[0]; x < lowerBounds[0] + dimensions[0]; x++)
            {
                for (int y = lowerBounds[1]; y < lowerBounds[1] + dimensions[1]; y++)
                {
                    for (int z = lowerBounds[2]; z < lowerBounds[2] + dimensions[2]; z++)
                    {
                        arrayWithLowerBounds.SetValue(value++, x, y, z);
                    }
                }
            }

            var tensor = new DenseTensor<int>(arrayWithLowerBounds, columnMajor);

            var expected = new DenseTensor<int>(new[,,]
                    {
                        {
                            { 0, 1, 2, 3 },
                            { 4, 5, 6, 7 },
                            { 8, 9, 10, 11 }
                        },
                        {
                            { 12, 13, 14, 15 },
                            { 16, 17, 18, 19 },
                            { 20, 21, 22, 23 }
                        }
                    }
                );
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(expected, tensor));
            Assert.Equal(columnMajor, tensor.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void StructurallyEqualTensor(bool columnMajor, bool columnMajor2)
        {
            var arr = new[, ,]
            {
                {
                    {0, 1, 2},
                    {3, 4, 5}
                },
                {
                    {6, 7 ,8 },
                    {9, 10 ,11 },
                },
                {
                    {12, 13 ,14 },
                    {15, 16 ,17 },
                },
                {
                    {18, 19 ,20 },
                    {21, 22 ,23 },
                }
            };
            var tensor = new DenseTensor<int>(arr, columnMajor);
            var tensor2 = new DenseTensor<int>(arr, columnMajor2);

            Assert.Equal(0, StructuralComparisons.StructuralComparer.Compare(tensor, tensor2));
            Assert.Equal(0, StructuralComparisons.StructuralComparer.Compare(tensor2, tensor));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, tensor2));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor2, tensor));
            // Issue: should Tensors with different layout be structurally equal?
            if (columnMajor == columnMajor2)
            {
                Assert.Equal(StructuralComparisons.StructuralEqualityComparer.GetHashCode(tensor), StructuralComparisons.StructuralEqualityComparer.GetHashCode(tensor2));
            }
        }

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void StructurallyEqualArray(bool columnMajor)
        {
            var arr = new[, ,]
            {
                {
                    {0, 1, 2},
                    {3, 4, 5}
                },
                {
                    {6, 7 ,8 },
                    {9, 10 ,11 },
                },
                {
                    {12, 13 ,14 },
                    {15, 16 ,17 },
                },
                {
                    {18, 19 ,20 },
                    {21, 22 ,23 },
                }
            };
            var tensor = new DenseTensor<int>(arr, columnMajor);

            Assert.Equal(0, StructuralComparisons.StructuralComparer.Compare(tensor, arr));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, arr));

        }

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void GetDiagonalSquare(bool columnMajor)
        {
            var arr = new[,]
            {
               { 1, 2, 4 },
               { 8, 3, 9 },
               { 1, 7, 5 },
            };

            var tensor = new DenseTensor<int>(arr, columnMajor);
            var diag = tensor.GetDiagonal();
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 1, 3, 5 }));
            diag = tensor.GetDiagonal(1);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 2, 9 }));
            diag = tensor.GetDiagonal(2);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 4 }));
            Assert.Throws<ArgumentException>("offset", () => tensor.GetDiagonal(3));

            diag = tensor.GetDiagonal(-1);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 8, 7 }));
            diag = tensor.GetDiagonal(-2);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 1 }));
            Assert.Throws<ArgumentException>("offset", () => tensor.GetDiagonal(-3));
        }

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void GetDiagonalRectangle(bool columnMajor)
        {
            var arr = new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 8, 3, 9, 2, 6 },
               { 1, 7, 5, 2, 9 }
            };

            var tensor = new DenseTensor<int>(arr, columnMajor);
            var diag = tensor.GetDiagonal();
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 1, 3, 5 }));
            diag = tensor.GetDiagonal(1);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 2, 9, 2 }));
            diag = tensor.GetDiagonal(2);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 4, 2, 9 }));
            diag = tensor.GetDiagonal(3);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 3, 6 }));
            diag = tensor.GetDiagonal(4);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 7 }));
            Assert.Throws<ArgumentException>("offset", () => tensor.GetDiagonal(5));

            diag = tensor.GetDiagonal(-1);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 8, 7 }));
            diag = tensor.GetDiagonal(-2);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 1 }));
            Assert.Throws<ArgumentException>("offset", () => tensor.GetDiagonal(-3));
            Assert.Throws<ArgumentException>("offset", () => tensor.GetDiagonal(-4));
            Assert.Throws<ArgumentException>("offset", () => tensor.GetDiagonal(-5));
        }


        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void GetDiagonalCube(bool columnMajor)
        {
            var arr = new[, ,]
            {
                {
                   { 1, 2, 4 },
                   { 8, 3, 9 },
                   { 1, 7, 5 },
                },
                {
                   { 4, 5, 7 },
                   { 1, 6, 2 },
                   { 3, 0, 8 },
                },
                {
                   { 5, 6, 1 },
                   { 2, 2, 3 },
                   { 4, 9, 4 },
                },

            };

            var tensor = new DenseTensor<int>(arr, columnMajor);
            var diag = tensor.GetDiagonal();
            var expected = new[,]
            {
                { 1, 2, 4 },
                { 1, 6, 2 },
                { 4, 9, 4 }
            };
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, expected));
            Assert.Equal(columnMajor, diag.IsReveresedStride);
        }

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void GetTriangleSquare(bool columnMajor)
        {
            var arr = new[,]
            {
               { 1, 2, 4 },
               { 8, 3, 9 },
               { 1, 7, 5 },
            };

            var tensor = new DenseTensor<int>(arr, columnMajor);
            var tri = tensor.GetTriangle(0);
            Assert.Equal(columnMajor, tri.IsReveresedStride);

            var expected = new DenseTensor<int>(new[,]
            {
               { 1, 0, 0 },
               { 8, 3, 0 },
               { 1, 7, 5 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(1);
            expected = new DenseTensor<int>(new[,]
            {
               { 1, 2, 0 },
               { 8, 3, 9 },
               { 1, 7, 5 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(2);
            expected = new DenseTensor<int>(new[,]
            {
               { 1, 2, 4 },
               { 8, 3, 9 },
               { 1, 7, 5 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetTriangle(3);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetTriangle(200);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetTriangle(-1);
            expected = new DenseTensor<int>(new[,]
            {
               { 0, 0, 0 },
               { 8, 0, 0 },
               { 1, 7, 0 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(-2);
            expected = new DenseTensor<int>(new[,]
            {
               { 0, 0, 0 },
               { 0, 0, 0 },
               { 1, 0, 0 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));


            expected = new DenseTensor<int>(new[,]
            {
               { 0, 0, 0 },
               { 0, 0, 0 },
               { 0, 0, 0 },
            });
            tri = tensor.GetTriangle(-3);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            // same as -3, should it be an exception?
            tri = tensor.GetTriangle(-4);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(-300);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
        }

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void GetTriangleRectangle(bool columnMajor)
        {
            var arr = new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 8, 3, 9, 2, 6 },
               { 1, 7, 5, 2, 9 }
            };

            var tensor = new DenseTensor<int>(arr, columnMajor);
            var tri = tensor.GetTriangle(0);
            var expected = new DenseTensor<int>(new[,]
            {
               { 1, 0, 0, 0, 0 },
               { 8, 3, 0, 0, 0 },
               { 1, 7, 5, 0, 0 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            Assert.Equal(columnMajor, tri.IsReveresedStride);

            tri = tensor.GetTriangle(1);
            expected = new DenseTensor<int>(new[,]
            {
               { 1, 2, 0, 0, 0 },
               { 8, 3, 9, 0, 0 },
               { 1, 7, 5, 2, 0 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(2);
            expected = new DenseTensor<int>(new[,]
            {
               { 1, 2, 4, 0, 0 },
               { 8, 3, 9, 2, 0 },
               { 1, 7, 5, 2, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(3);
            expected = new DenseTensor<int>(new[,]
            {
               { 1, 2, 4, 3, 0 },
               { 8, 3, 9, 2, 6 },
               { 1, 7, 5, 2, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetTriangle(4);
            expected = new DenseTensor<int>(new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 8, 3, 9, 2, 6 },
               { 1, 7, 5, 2, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            // same as 4, should it be an exception?
            tri = tensor.GetTriangle(5);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(1000);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetTriangle(-1);
            expected = new DenseTensor<int>(new[,]
            {
               { 0, 0, 0, 0, 0 },
               { 8, 0, 0, 0, 0 },
               { 1, 7, 0, 0, 0 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            expected = new DenseTensor<int>(new[,]
            {
               { 0, 0, 0, 0, 0 },
               { 0, 0, 0, 0, 0 },
               { 1, 0, 0, 0, 0 }
            });
            tri = tensor.GetTriangle(-2);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            expected = new DenseTensor<int>(new[,]
            {
               { 0, 0, 0, 0, 0 },
               { 0, 0, 0, 0, 0 },
               { 0, 0, 0, 0, 0 }
            });
            tri = tensor.GetTriangle(-3);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetTriangle(-4);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(-5);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(-100);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
        }

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void GetTriangleCube(bool columnMajor)
        {
            var arr = new[, ,]
            {
                {
                   { 1, 2, 4 },
                   { 8, 3, 9 },
                   { 1, 7, 5 },
                },
                {
                   { 4, 5, 7 },
                   { 1, 6, 2 },
                   { 3, 0, 8 },
                },
                {
                   { 5, 6, 1 },
                   { 2, 2, 3 },
                   { 4, 9, 4 },
                },

            };

            var tensor = new DenseTensor<int>(arr, columnMajor);
            var tri = tensor.GetTriangle(0);
            var expected = new DenseTensor<int>(new[,,]
            {
                {
                   { 1, 2, 4 },
                   { 0, 0, 0 },
                   { 0, 0, 0 },
                },
                {
                   { 4, 5, 7 },
                   { 1, 6, 2 },
                   { 0, 0, 0 },
                },
                {
                   { 5, 6, 1 },
                   { 2, 2, 3 },
                   { 4, 9, 4 },
                },

            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            Assert.Equal(columnMajor, tri.IsReveresedStride);
        }

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void GetUpperTriangleSquare(bool columnMajor)
        {
            var arr = new[,]
            {
               { 1, 2, 4 },
               { 8, 3, 9 },
               { 1, 7, 5 },
            };

            var tensor = new DenseTensor<int>(arr, columnMajor);
            var tri = tensor.GetUpperTriangle(0);

           var expected = new DenseTensor<int>(new[,]
            {
               { 1, 2, 4 },
               { 0, 3, 9 },
               { 0, 0, 5 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            Assert.Equal(columnMajor, tri.IsReveresedStride);

            tri = tensor.GetUpperTriangle(1);
            expected = new DenseTensor<int>(new[,]
            {
               { 0, 2, 4 },
               { 0, 0, 9 },
               { 0, 0, 0 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(2);
            expected = new DenseTensor<int>(new[,]
            {
               { 0, 0, 4 },
               { 0, 0, 0 },
               { 0, 0, 0 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetUpperTriangle(3);
            expected = new DenseTensor<int>(new[,]
            {
               { 0, 0, 0 },
               { 0, 0, 0 },
               { 0, 0, 0 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetUpperTriangle(4);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(42);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetUpperTriangle(-1);
            expected = new DenseTensor<int>(new[,]
            {
               { 1, 2, 4 },
               { 8, 3, 9 },
               { 0, 7, 5 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(-2);
            expected = new DenseTensor<int>(new[,]
            {
               { 1, 2, 4 },
               { 8, 3, 9 },
               { 1, 7, 5 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetUpperTriangle(-3);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(-300);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
        }

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void GetUpperTriangleRectangle(bool columnMajor)
        {
            var arr = new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 8, 3, 9, 2, 6 },
               { 1, 7, 5, 2, 9 }
            };

            var tensor = new DenseTensor<int>(arr, columnMajor);
            var tri = tensor.GetUpperTriangle(0);
            var expected = new DenseTensor<int>(new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 0, 3, 9, 2, 6 },
               { 0, 0, 5, 2, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            Assert.Equal(columnMajor, tri.IsReveresedStride);
            tri = tensor.GetUpperTriangle(1);
            expected = new DenseTensor<int>(new[,]
            {
               { 0, 2, 4, 3, 7 },
               { 0, 0, 9, 2, 6 },
               { 0, 0, 0, 2, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(2);
            expected = new DenseTensor<int>(new[,]
            {
               { 0, 0, 4, 3, 7 },
               { 0, 0, 0, 2, 6 },
               { 0, 0, 0, 0, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(3);
            expected = new DenseTensor<int>(new[,]
            {
               { 0, 0, 0, 3, 7 },
               { 0, 0, 0, 0, 6 },
               { 0, 0, 0, 0, 0 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetUpperTriangle(4);
            expected = new DenseTensor<int>(new[,]
            {
               { 0, 0, 0, 0, 7 },
               { 0, 0, 0, 0, 0 },
               { 0, 0, 0, 0, 0 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            expected = new DenseTensor<int>(new[,]
            {
               { 0, 0, 0, 0, 0 },
               { 0, 0, 0, 0, 0 },
               { 0, 0, 0, 0, 0 }
            });
            tri = tensor.GetUpperTriangle(5);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(6);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(1000);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetUpperTriangle(-1);
            expected = new DenseTensor<int>(new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 8, 3, 9, 2, 6 },
               { 0, 7, 5, 2, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            expected = new DenseTensor<int>(new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 8, 3, 9, 2, 6 },
               { 1, 7, 5, 2, 9 }
            });
            tri = tensor.GetUpperTriangle(-2);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            
            tri = tensor.GetUpperTriangle(-3);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(-4);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(-100);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
        }

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void GetUpperTriangleCube(bool columnMajor)
        {
            var arr = new[, ,]
            {
                {
                   { 1, 2, 4 },
                   { 8, 3, 9 },
                   { 1, 7, 5 },
                },
                {
                   { 4, 5, 7 },
                   { 1, 6, 2 },
                   { 3, 0, 8 },
                },
                {
                   { 5, 6, 1 },
                   { 2, 2, 3 },
                   { 4, 9, 4 },
                },

            };

            var tensor = new DenseTensor<int>(arr, columnMajor);
            var tri = tensor.GetUpperTriangle(0);
            var expected = new DenseTensor<int>(new[, ,]
            {
                {
                   { 1, 2, 4 },
                   { 8, 3, 9 },
                   { 1, 7, 5 },
                },
                {
                   { 0, 0, 0 },
                   { 1, 6, 2 },
                   { 3, 0, 8 },
                },
                {
                   { 0, 0, 0 },
                   { 0, 0, 0 },
                   { 4, 9, 4 },
                },

            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            Assert.Equal(columnMajor, tri.IsReveresedStride);
        }

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void Reshape(bool columnMajor)
        {
            var arr = new[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            var tensor = new DenseTensor<int>(arr, columnMajor);
            var actual = tensor.Reshape(3, 2);

            var expected = columnMajor ?
                new[,]
                {
                    { 1, 5 },
                    { 4, 3 },
                    { 2, 6 }
                } :
                new[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                    { 5, 6 }
                };
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(columnMajor, actual.IsReveresedStride);
        }

        [Fact]
        public void Identity()
        {
            var actual = Tensor<double>.CreateIdentity(3);

            var expected = new[,]
            {
                {1.0, 0, 0 },
                {0, 1.0, 0 },
                {0, 0, 1.0 }
            };

            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void CreateWithDiagonal()
        {
            var actual = Tensor<int>.CreateFromDiagonal(new DenseTensor<int>(fromArray: new[] { 1, 2, 3, 4, 5 }));

            var expected = new[,]
            {
                {1, 0, 0, 0, 0 },
                {0, 2, 0, 0, 0 },
                {0, 0, 3, 0, 0 },
                {0, 0, 0, 4, 0 },
                {0, 0, 0, 0, 5 }
            };

            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void CreateWithDiagonalAndOffset()
        {
            var actual = Tensor<int>.CreateFromDiagonal(new DenseTensor<int>(fromArray: new[] { 1, 2, 3, 4 }), 1);

            var expected = new[,]
            {
                {0, 1, 0, 0, 0 },
                {0, 0, 2, 0, 0 },
                {0, 0, 0, 3, 0 },
                {0, 0, 0, 0, 4 },
                {0, 0, 0, 0, 0 }
            };

            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));

            actual = Tensor<int>.CreateFromDiagonal(new DenseTensor<int>(fromArray: new[] { 1, 2, 3, 4 }), -1);

            expected = new[,]
            {
                {0, 0, 0, 0, 0 },
                {1, 0, 0, 0, 0 },
                {0, 2, 0, 0, 0 },
                {0, 0, 3, 0, 0 },
                {0, 0, 0, 4, 0 }
            };

            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));

            actual = Tensor<int>.CreateFromDiagonal(new DenseTensor<int>(fromArray: new[] { 1 }), -4);
            expected = new[,]
            {
                {0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0 },
                {1, 0, 0, 0, 0 }
            };
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));

            actual = Tensor<int>.CreateFromDiagonal(new DenseTensor<int>(fromArray: new[] { 1 }), 4);
            expected = new[,]
            {
                {0, 0, 0, 0, 1 },
                {0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0 }
            };
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Theory()]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Add(bool leftColumnMajor, bool rightColumnMajor)
        {
            var left = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, leftColumnMajor);
            var right = new DenseTensor<int>(
                new[,]
                {
                    { 6, 7 ,8 },
                    { 9, 10 ,11 },
                }, rightColumnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    { 6, 8, 10 },
                    { 12, 14, 16 },
                });

            var actual = left + right;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(leftColumnMajor, actual.IsReveresedStride);

        }

        [Theory()]
        [InlineData(false)]
        [InlineData(true)]
        public void AddScalar(bool columnMajor)
        {
            var tensor = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, columnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 },
                });

            var actual = tensor + 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(columnMajor, actual.IsReveresedStride);

        }

        [Theory()]
        [InlineData(false)]
        [InlineData(true)]
        public void UnaryPlus(bool columnMajor)
        {
            var tensor = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, columnMajor);

            var expected = tensor;

            var actual = +tensor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(false, ReferenceEquals(actual, expected));
            Assert.Equal(columnMajor, actual.IsReveresedStride);
        }


        [Theory()]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Subtract(bool leftColumnMajor, bool rightColumnMajor)
        {
            var left = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, leftColumnMajor);
            var right = new DenseTensor<int>(
                new[,]
                {
                    { 6, 7 ,8 },
                    { 9, 10 ,11 },
                }, rightColumnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    { -6, -6, -6 },
                    { -6, -6, -6},
                });

            var actual = left - right;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(leftColumnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false)]
        [InlineData(true)]
        public void SubtractScalar(bool columnMajor)
        {
            var tensor = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, columnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    { -1, 0, 1 },
                    { 2, 3, 4 },
                });

            var actual = tensor - 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(columnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false)]
        [InlineData(true)]
        public void UnaryMinus(bool columnMajor)
        {
            var tensor = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, columnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    {0, -1, -2},
                    {-3, -4, -5}
                });

            var actual = -tensor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(false, ReferenceEquals(actual, expected));
            Assert.Equal(columnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false)]
        [InlineData(true)]
        public void PrefixIncrement(bool columnMajor)
        {
            Tensor<int> tensor = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, columnMajor);

            var expectedResult = new DenseTensor<int>(
                new[,]
                {
                    {1, 2, 3},
                    {4, 5, 6}
                });

            var expectedTensor = expectedResult;

            var actual = ++tensor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expectedResult));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, expectedTensor));
            Assert.Equal(true, ReferenceEquals(tensor, actual));
            Assert.Equal(columnMajor, actual.IsReveresedStride);
        }


        [Theory()]
        [InlineData(false)]
        [InlineData(true)]
        public void PostfixIncrement(bool columnMajor)
        {
            Tensor<int> tensor = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, columnMajor);

            // returns original value
            var expectedResult = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            // increments operand
            var expectedTensor = new DenseTensor<int>(
                new[,]
                {
                    {1, 2, 3},
                    {4, 5, 6}
                }); ;

            var actual = tensor++;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expectedResult));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, expectedTensor));
            Assert.Equal(false, ReferenceEquals(tensor, actual));
            Assert.Equal(columnMajor, actual.IsReveresedStride);
        }


        [Theory()]
        [InlineData(false)]
        [InlineData(true)]
        public void PrefixDecrement(bool columnMajor)
        {
            Tensor<int> tensor = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, columnMajor);

            var expectedResult = new DenseTensor<int>(
                new[,]
                {
                    {-1, 0, 1},
                    {2, 3, 4}
                });

            var expectedTensor = expectedResult;

            var actual = --tensor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expectedResult));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, expectedTensor));
            Assert.Equal(true, ReferenceEquals(tensor, actual));
            Assert.Equal(columnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false)]
        [InlineData(true)]
        public void PostfixDecrement(bool columnMajor)
        {
            Tensor<int> tensor = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, columnMajor);

            // returns original value
            var expectedResult = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            // decrements operand
            var expectedTensor = new DenseTensor<int>(
                new[,]
                {
                    {-1, 0, 1},
                    {2, 3, 4}
                }); ;

            var actual = tensor--;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expectedResult));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, expectedTensor));
            Assert.Equal(false, ReferenceEquals(tensor, actual));
            Assert.Equal(columnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Multiply(bool leftColumnMajor, bool rightColumnMajor)
        {
            var left = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, leftColumnMajor);
            var right = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, rightColumnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 4},
                    {9, 16, 25}
                });

            var actual = left * right;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(leftColumnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false)]
        [InlineData(true)]
        public void MultiplyScalar(bool columnMajor)
        {
            var tensor = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, columnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    {0, 2, 4},
                    {6, 8, 10}
                });

            var actual = tensor * 2;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(columnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Divide(bool dividendColumnMajor, bool divisorColumnMajor)
        {
            var dividend = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 4},
                    {9, 16, 25}
                }, dividendColumnMajor);

            var divisor = new DenseTensor<int>(
                new[,]
                {
                    {1, 1, 2},
                    {3, 4, 5}
                }, divisorColumnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var actual = dividend / divisor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(dividendColumnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false)]
        [InlineData(true)]
        public void DivideScalar(bool columnMajor)
        {
            var tensor = new DenseTensor<int>(
                new[,]
                {
                    {0, 2, 4},
                    {6, 8, 10}
                }, columnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var actual = tensor / 2;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(columnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Modulo(bool dividendColumnMajor, bool divisorColumnMajor)
        {
            var dividend = new DenseTensor<int>(
                new[,]
                {
                    {0, 3, 8},
                    {11, 14, 17}
                }, dividendColumnMajor);

            var divisor = new DenseTensor<int>(
                new[,]
                {
                    {1, 2, 3},
                    {4, 5, 6}
                }, divisorColumnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var actual = dividend % divisor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(dividendColumnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false)]
        [InlineData(true)]
        public void ModuloScalar(bool columnMajor)
        {
            var tensor = new DenseTensor<int>(
                new[,]
                {
                    {0, 3, 4},
                    {7, 8, 9}
                }, columnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 0},
                    {1, 0, 1}
                });

            var actual = tensor % 2;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(columnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void And(bool leftColumnMajor, bool rightColumnMajor)
        {
            var left = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 3},
                    {7, 15, 31}
                }, leftColumnMajor);

            var right = new DenseTensor<int>(
                new[,]
                {
                    {1, 1, 3},
                    {2, 4, 8}
                }, rightColumnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 3},
                    {2, 4, 8}
                });

            var actual = left & right;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(leftColumnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false)]
        [InlineData(true)]
        public void AndScalar(bool columnMajor)
        {
            var left = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 3},
                    {5, 15, 31}
                }, columnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    {0, 0, 0},
                    {4, 4, 20}
                });

            var actual = left & 20;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(columnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Or(bool leftColumnMajor, bool rightColumnMajor)
        {
            var left = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 3},
                    {7, 14, 31}
                }, leftColumnMajor);

            var right = new DenseTensor<int>(
                new[,]
                {
                    {1, 2, 4},
                    {2, 4, 8}
                }, rightColumnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    {1, 3, 7},
                    {7, 14, 31}
                });

            var actual = left | right;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(leftColumnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false)]
        [InlineData(true)]
        public void OrScalar(bool columMajor)
        {
            var left = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, columMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    {1, 1, 3},
                    {3, 5, 5}
                });

            var actual = left | 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(columMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Xor(bool leftColumnMajor, bool rightColumnMajor)
        {
            var left = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 3},
                    {7, 14, 31}
                }, leftColumnMajor);

            var right = new DenseTensor<int>(
                new[,]
                {
                    {1, 2, 4},
                    {2, 4, 8}
                }, rightColumnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    {1, 3, 7},
                    {5, 10, 23}
                });

            var actual = left ^ right;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(leftColumnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false)]
        [InlineData(true)]
        public void XorScalar(bool columnMajor)
        {
            var left = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, columnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    {1, 0, 3},
                    {2, 5, 4}
                });

            var actual = left ^ 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(columnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false)]
        [InlineData(true)]
        public void LeftShift(bool columnMajor)
        {
            var left = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, columnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    {0, 2, 4},
                    {6, 8, 10}
                });

            var actual = left << 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(columnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false)]
        [InlineData(true)]
        public void RightShift(bool columnMajor)
        {
            var left = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, columnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    {0, 0, 1},
                    {1, 2, 2}
                });

            var actual = left >> 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(columnMajor, actual.IsReveresedStride);
        }

        [Theory()]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void ElementWiseEquals(bool leftColumnMajor, bool rightColumnMajor)
        {
            var left = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, leftColumnMajor);
            var right = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, -2},
                    {2, 3, 5}
                }, rightColumnMajor);

            var expected = new DenseTensor<bool>(
                new[,]
                {
                    {true, true, false },
                    {false, false, true}
                });

            var actual = Tensor.Equals(left, right);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(leftColumnMajor, actual.IsReveresedStride);
        }
        
        [Theory()]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void ElementWiseNotEquals(bool leftColumnMajor, bool rightColumnMajor)
        {
            var left = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, leftColumnMajor);
            var right = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, -2},
                    {2, 3, 5}
                }, rightColumnMajor);

            var expected = new DenseTensor<bool>(
                new[,]
                {
                    {false, false, true},
                    {true, true, false}
                });

            var actual = Tensor.NotEquals(left, right);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(leftColumnMajor, actual.IsReveresedStride);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void MatrixMultiply(bool leftColumnMajor, bool rightColumnMajor)
        {
            var left = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                }, leftColumnMajor);

            var right = new DenseTensor<int>(
                new[,]
                {
                    {0, 1, 2, 3, 4},
                    {5, 6, 7, 8, 9},
                    {10, 11, 12, 13, 14}
                }, rightColumnMajor);

            var expected = new DenseTensor<int>(
                new[,]
                {
                    {0*0 + 1*5 + 2*10, 0*1 + 1*6 + 2*11, 0*2 + 1*7 + 2*12, 0*3 + 1*8 + 2*13, 0*4 + 1*9 + 2*14},
                    {3*0 + 4*5 + 5*10, 3*1 + 4*6 + 5*11, 3*2 + 4*7 + 5*12, 3*3 + 4*8 + 5*13, 3*4 + 4*9 + 5*14}
                });

            var actual = left.MatrixMultiply(right);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void Contract(bool leftColumnMajor, bool rightColumnMajor)
        {
            var left = new DenseTensor<int>(
                new[, ,]
                {
                    {
                        {0, 1},
                        {2, 3}
                    },
                    {
                        {4, 5},
                        {6, 7}
                    },
                    {
                        {8, 9},
                        {10, 11}
                    }
                }, leftColumnMajor);

            var right = new DenseTensor<int>(
                new[, ,]
                {
                    {
                        {0, 1},
                        {2, 3},
                        {4, 5}
                    },
                    {
                        {6, 7},
                        {8, 9},
                        {10, 11}
                    },
                    {
                        {12, 13},
                        {14, 15},
                        {16, 17}
                    },
                    {
                        {18, 19},
                        {20, 21},
                        {22, 23}
                    }
                }, rightColumnMajor);

            // contract a 3*2*2 with a 4*3*2 tensor, summing on (3*2)*2 and 4*(3*2) to produce a 2*4 tensor
            var expected = new DenseTensor<int>(
                new[,]
                {
                    {110, 290, 470, 650},
                    {125, 341, 557, 773},
                });
            var actual = Tensor.Contract(left, right, new[] { 0, 1 }, new[] { 1, 2 });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));

            // contract a 3*2*2 with a 4*3*2 tensor, summing on (3)*2*(2) and 4*(3*2) to produce a 2*4 tensor
            expected = new DenseTensor<int>(
                new[,]
                {
                    {101, 263, 425, 587},
                    {131, 365, 599, 833},
                });
            actual = Tensor.Contract(left, right, new[] { 0, 2 }, new[] { 1, 2 });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void ContractMismatchedDimensions()
        {
            var left = new DenseTensor<int>(fromArray:
                new[] { 0, 1, 2, 3 });

            var right = new DenseTensor<int>(
                new[,]
                {
                    { 0 },
                    { 1 },
                    { 2 }
                });

            var expected = new DenseTensor<int>(
                new[,]
                {
                    {0,0,0},
                    {0,1,2},
                    {0,2,4},
                    {0,3,6},
                });

            Assert.Throws<ArgumentException>(() => Tensor.Contract(left, right, new int[] { }, new[] { 1 }));

            // reshape to include dimension of length 1.
            var leftReshaped = left.Reshape(1, (int)left.Length);

            var actual = Tensor.Contract(leftReshaped, right, new[] { 0 }, new[] { 1 });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }
    }
}
