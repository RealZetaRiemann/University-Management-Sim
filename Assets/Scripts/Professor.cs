using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Professor
{
    public string Name;
    public string Specialty;
    public int SpecialtyIndex;
    public List<Course> Courses;
    public int InitialCost;
    public int UpkeepCost;

    public Professor(string newName, string newSpecialty, int newSpecialtyIndex, List<Course> newCourses, int newInitialCost, 
    int newUpkeepCost) 
    {
        Name = newName;
        Specialty = newSpecialty;
        SpecialtyIndex = newSpecialtyIndex;
        Courses = newCourses;
        InitialCost = newInitialCost;
        UpkeepCost = newUpkeepCost;
    }
}