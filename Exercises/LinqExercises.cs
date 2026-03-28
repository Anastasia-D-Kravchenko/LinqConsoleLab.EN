using LinqConsoleLab.EN.Data;

namespace LinqConsoleLab.EN.Exercises;

public sealed class LinqExercises
{
    public IEnumerable<string> Task01_StudentsFromWarsaw()
    {
        return UniversityData.Students
            .Where(s => s.City == "Warsaw")
            .Select(s => $"{s.IndexNumber} - {s.FirstName} {s.LastName}");
    }

    public IEnumerable<string> Task02_StudentEmailAddresses()
    {
        return UniversityData.Students
            .Select(s => s.Email);
    }

    public IEnumerable<string> Task03_StudentsSortedAlphabetically()
    {
        return UniversityData.Students
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .Select(s => $"{s.IndexNumber} - {s.FirstName} {s.LastName}");
    }

    public IEnumerable<string> Task04_FirstAnalyticsCourse()
    {
        var course = UniversityData.Courses.FirstOrDefault(c => c.Category == "Analytics");
        
        return course != null 
            ? new[] { $"{course.Title} ({course.StartDate:yyyy-MM-dd})" } 
            : new[] { "No Analytics course found." };
    }

    public IEnumerable<string> Task05_IsThereAnyInactiveEnrollment()
    {
        bool hasInactive = UniversityData.Enrollments.Any(e => !e.IsActive);
        return new[] { hasInactive ? "Yes" : "No" };
    }

    public IEnumerable<string> Task06_DoAllLecturersHaveDepartment()
    {
        bool allHaveDepartment = UniversityData.Lecturers.All(l => !string.IsNullOrWhiteSpace(l.Department));
        return new[] { allHaveDepartment ? "Yes" : "No" };
    }

    public IEnumerable<string> Task07_CountActiveEnrollments()
    {
        int count = UniversityData.Enrollments.Count(e => e.IsActive);
        return new[] { count.ToString() };
    }

    public IEnumerable<string> Task08_DistinctStudentCities()
    {
        return UniversityData.Students
            .Select(s => s.City)
            .Distinct()
            .OrderBy(city => city);
    }

    public IEnumerable<string> Task09_ThreeNewestEnrollments()
    {
        return UniversityData.Enrollments
            .OrderByDescending(e => e.EnrollmentDate)
            .Take(3)
            .Select(e => $"{e.EnrollmentDate:yyyy-MM-dd} | Student: {e.StudentId} | Course: {e.CourseId}");
    }

    public IEnumerable<string> Task10_SecondPageOfCourses()
    {
        return UniversityData.Courses
            .OrderBy(c => c.Title)
            .Skip(2)
            .Take(2)
            .Select(c => $"{c.Title} ({c.Category})");
    }

    public IEnumerable<string> Task11_JoinStudentsWithEnrollments()
    {
        return UniversityData.Students
            .Join(UniversityData.Enrollments, 
                s => s.Id, 
                e => e.StudentId, 
                (s, e) => $"{s.FirstName} {s.LastName} enrolled on {e.EnrollmentDate:yyyy-MM-dd}");
    }

    public IEnumerable<string> Task12_StudentCoursePairs()
    {
        return from e in UniversityData.Enrollments
            join s in UniversityData.Students on e.StudentId equals s.Id
            join c in UniversityData.Courses on e.CourseId equals c.Id
            select $"{s.FirstName} {s.LastName} - {c.Title}";
    }

    public IEnumerable<string> Task13_GroupEnrollmentsByCourse()
    {
        return from e in UniversityData.Enrollments
            join c in UniversityData.Courses on e.CourseId equals c.Id
            group e by c.Title into g
            select $"{g.Key}: {g.Count()} enrollments";
    }

    public IEnumerable<string> Task14_AverageGradePerCourse()
    {
        return from e in UniversityData.Enrollments
            where e.FinalGrade.HasValue
            join c in UniversityData.Courses on e.CourseId equals c.Id
            group e by c.Title into g
            select $"{g.Key}: {g.Average(e => e.FinalGrade):F2}";
    }

    public IEnumerable<string> Task15_LecturersAndCourseCounts()
    {
        return UniversityData.Lecturers
            .GroupJoin(UniversityData.Courses, 
                l => l.Id, 
                c => c.LecturerId, 
                (l, courses) => $"{l.FirstName} {l.LastName}: {courses.Count()} courses");
    }

    public IEnumerable<string> Task16_HighestGradePerStudent()
    {
        return from e in UniversityData.Enrollments
            where e.FinalGrade.HasValue
            join s in UniversityData.Students on e.StudentId equals s.Id
            group e by new { s.FirstName, s.LastName } into g
            select $"{g.Key.FirstName} {g.Key.LastName}: {g.Max(e => e.FinalGrade):F2}";
    }

    /// <summary>
    /// Challenge:
    /// Find students who have more than one active enrollment.
    /// Return the full name and the number of active courses.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, COUNT(*)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.IsActive = 1
    /// GROUP BY s.FirstName, s.LastName
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Challenge01_StudentsWithMoreThanOneActiveCourse()
    {
        throw NotImplemented(nameof(Challenge01_StudentsWithMoreThanOneActiveCourse));
    }

    /// <summary>
    /// Challenge:
    /// List the courses that start in April 2026 and do not have any final grades assigned yet.
    ///
    /// SQL:
    /// SELECT c.Title
    /// FROM Courses c
    /// JOIN Enrollments e ON c.Id = e.CourseId
    /// WHERE MONTH(c.StartDate) = 4 AND YEAR(c.StartDate) = 2026
    /// GROUP BY c.Title
    /// HAVING SUM(CASE WHEN e.FinalGrade IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Challenge02_AprilCoursesWithoutFinalGrades()
    {
        throw NotImplemented(nameof(Challenge02_AprilCoursesWithoutFinalGrades));
    }

    /// <summary>
    /// Challenge:
    /// Calculate the average final grade for every lecturer across all of their courses.
    /// Ignore missing grades but still keep the lecturers in mind as the reporting dimension.
    ///
    /// SQL:
    /// SELECT l.FirstName, l.LastName, AVG(e.FinalGrade)
    /// FROM Lecturers l
    /// LEFT JOIN Courses c ON c.LecturerId = l.Id
    /// LEFT JOIN Enrollments e ON e.CourseId = c.Id
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY l.FirstName, l.LastName;
    /// </summary>
    public IEnumerable<string> Challenge03_LecturersAndAverageGradeAcrossTheirCourses()
    {
        throw NotImplemented(nameof(Challenge03_LecturersAndAverageGradeAcrossTheirCourses));
    }

    /// <summary>
    /// Challenge:
    /// Show student cities and the number of active enrollments created by students from each city.
    /// Sort the result by the active enrollment count in descending order.
    ///
    /// SQL:
    /// SELECT s.City, COUNT(*)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.IsActive = 1
    /// GROUP BY s.City
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Challenge04_CitiesAndActiveEnrollmentCounts()
    {
        throw NotImplemented(nameof(Challenge04_CitiesAndActiveEnrollmentCounts));
    }

    private static NotImplementedException NotImplemented(string methodName)
    {
        return new NotImplementedException(
            $"Complete method {methodName} in Exercises/LinqExercises.cs and run the command again.");
    }
}
