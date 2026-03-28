# LinqConsoleLab.EN - University Data Analysis

## Business Context
This project is a console-based analytical tool designed to support the everyday work of an academic coordination office. The application allows program coordinators to quickly find student information, check enrollment activities, and generate cross-sectional reports regarding courses and lecturers.

## Project Scope
The goal of this lab was to implement a series of data queries using **Language Integrated Query (LINQ)** in C# to interact with in-memory collections representing a university database.

### Implemented Features:
- **Data Filtering & Sorting**: Efficiently retrieving specific records using `.Where()`, `.OrderBy()`, and `.ThenBy()`.
- **Projection**: Using `.Select()` to transform data objects into user-friendly string outputs.
- **Aggregation**: Calculating business metrics using `.Count()`, `.Average()`, and `.Max()`.
- **Advanced Joins**: Combining disparate collections (Students, Courses, Lecturers, Enrollments) using `.Join()` and `.GroupJoin()`.
- **Pagination**: Implementing data paging for large sets using `.Skip()` and `.Take()`.
- **Complex Reporting**: Solving high-level "Challenge" tasks such as identifying students with multiple active enrollments and calculating average grades across a lecturer's entire portfolio.

## Technical Stack
- **Language**: C#
- **Framework**: .NET 8.0 / Console Application
- **Technology**: LINQ (Method & Query Syntax)

## Project Structure
- `Models/`: Contains domain entities (`Student`, `Course`, `Lecturer`, `Enrollment`).
- `Data/`: Contains the `UniversityData` class responsible for data seeding and overview.
- `Exercises/`: The core logic where LINQ queries are implemented in `LinqExercises.cs`.

## How to Run
1. Ensure you have the .NET SDK installed.
2. Clone the repository:
   ```bash
   git clone [https://github.com/Anastasia-D-Kravchenko/LinqConsoleLab.EN.git](https://github.com/Anastasia-D-Kravchenko/LinqConsoleLab.EN.git)
   ```
3. Navigate to the project directory and run:
   ```bash
    dotnet run
   ```
4. Use the on-screen menu (0-20) to execute different data analysis tasks.