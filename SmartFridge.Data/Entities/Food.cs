/*
Class: Food
Purpose: Serve as an entity class for the food objects
Classify variables such as dates, type of food, name, price, calories, protein, carbs etc.
*/

using System;
using Bogus.DataSets;


namespace SmartFridge.Data.Entities
{
    public class Food
    {
        int foodID;
        string nameOfFood;
        DateTime today = new DateTime();
        DateTime useByDate = new DateTime();
        int daysLeft;
        int calories;
        int protein;
        int fibre;
        int carbs;
        double price;
    }
}