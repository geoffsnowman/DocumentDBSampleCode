--These queries assume that a collection has been created that contains the food nutrition 
--sample data from https://azure.microsoft.com/en-us/blog/import-sample-data-to-azure-documentdb/

SELECT *
FROM food
WHERE food.foodGroup = "Snacks" and food.id = "19015"

SELECT food.id,
	food.description,
	food.tags,
	food.foodGroup,
	food.manufacturerName,
	food.version
FROM food
WHERE (food.manufacturerName = "The Coca-Cola Company" AND food.version > 0)

SELECT food.description, 
	food.foodGroup, 
	food.servings[0].description AS servingDescription, 
	food.servings[0].weightInGrams AS servingWeight 
FROM food 
WHERE food.foodGroup = "Fruits and Fruit Juices"
	AND food.servings[0].description = "cup"
ORDER BY food.servings[0].weightInGrams DESC

SELECT TOP 20 food.id, 
	food.description, 
	food.tags, 
	food.foodGroup 
FROM food 
WHERE food.foodGroup = "Snacks"
ORDER BY food.description ASC

SELECT food.id,
	food.description,
	food.tags,
	food.foodGroup,
	food.version 
FROM food 
WHERE food.foodGroup 
	IN ("Poultry Products", "Sausages and Luncheon Meats")
	AND (food.id BETWEEN "05740" AND "07050")

SELECT TOP 30 food.id,
    food.description,
    food.foodGroup 
FROM food
WHERE STARTSWITH(food.foodGroup, 'Baby')

SELECT food.description, 
	food.tags 
FROM food 
WHERE ARRAY_LENGTH(food.tags) > 6

SELECT { 
	"Company": food.manufacturerName,
	"Brand": food.commonName,
	"Serving Description": food.servings[0].description,
	"Serving in Grams": food.servings[0].weightInGrams,
	"Food Group": food.foodGroup 
	} AS Food
FROM food
WHERE food.id = "21421"

SELECT tag.name
FROM food
JOIN tag IN food.tags
WHERE food.id = "09052"

SELECT food.id, 
	food.commonName, 
    food.description,
	food.foodGroup, 
	ROUND(nutrient.nutritionValue) AS amount, 
	nutrient.units 
FROM food JOIN nutrient IN food.nutrients 
WHERE IS_DEFINED(food.commonName) 
	AND nutrient.description = "Water" 
	AND food.foodGroup IN ("Sausages and Luncheon Meats", "Legumes and Legume Products")
ORDER BY food.description ASC


