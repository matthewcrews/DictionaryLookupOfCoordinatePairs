A repo for a series of videos on the performance of Key/Value lookup of values in a Dictionary.

Current benchmark results:

|       Method |      Mean |    Error |   StdDev |
|------------- |----------:|---------:|---------:|
| DefaultTuple | 134.92 us | 2.641 us | 5.025 us |
|  StructTuple |  25.52 us | 0.336 us | 0.298 us |
|   PairRecord |  17.83 us | 0.226 us | 0.189 us |
| StructRecord |  17.81 us | 0.332 us | 0.326 us |