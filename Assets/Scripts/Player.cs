using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Professor> EnlistedProfs;
    public List<Course> AvailableCourses;
    public List<Student> CurrentStudents;
    public List<(string, int)> AvailableMajors;
    public int Prestige;
    public int GP;
    public int Academics;
    public int numStudents;

    public Player()
    {
        EnlistedProfs = new List<Professor>();
        AvailableCourses = new List<Course>();
        CurrentStudents = new List<Student>();
        AvailableMajors = new List<(string, int)>();
        Prestige = 0;
        GP = 0;
        Academics = 0;
        numStudents = 0;
    }
}
