using TB5.ConsoleApp;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

Console.WriteLine("--- RestBird API Service Demo (using RestSharp) ---");

RestBirdApiService apiService = new RestBirdApiService();

// 1. Read All
Console.WriteLine("\n[1. Read All Birds]");
var birds = await apiService.Read();
foreach (var bird in birds.Take(5)) // Show first 5
{
    Console.WriteLine($"{bird.Id.ToString().PadRight(2)} : {bird.BirdEnglishName} ({bird.BirdMyanmarName})");
}

// 2. Read Single
Console.WriteLine("\n[2. Read Single Bird (ID: 1)]");
var singleBird = await apiService.Read(1);
if (singleBird != null)
{
    Console.WriteLine($"Found: {singleBird.BirdEnglishName}");
}

// 3. Create
Console.WriteLine("\n[3. Create Bird]");
var newBird = new BirdModel
{
    BirdEnglishName = "Sparrow",
    BirdMyanmarName = "ငှက်ငယ်",
    Description = "A small bird commonly found in urban areas.",
    ImagePath = "https://example.com/sparrow.jpg"
};
await apiService.Create(newBird);

// 4. Update
Console.WriteLine("\n[4. Update Bird (ID: 1)]");
if (singleBird != null)
{
    singleBird.Description = "Updated description for stork-billed kingfisher.";
    await apiService.Update(1, singleBird);
}

// 5. Patch
Console.WriteLine("\n[5. Patch Bird (ID: 2)]");
var patchModel = new BirdModel { BirdEnglishName = "Updated Kingfisher" };
await apiService.Patch(2, patchModel);

// 6. Delete
Console.WriteLine("\n[6. Delete Bird (ID: 3)]");
await apiService.Delete(3);

Console.WriteLine("\nDemo completed. Press Enter to exit.");
Console.ReadLine();