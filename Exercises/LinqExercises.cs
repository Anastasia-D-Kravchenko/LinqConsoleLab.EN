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

    public IEnumerable<string> Challenge01_StudentsWithMoreThanOneActiveCourse()
    {
        return from e in UniversityData.Enrollments
            where e.IsActive
            join s in UniversityData.Students on e.StudentId equals s.Id
            group e by new { s.FirstName, s.LastName } into g
            where g.Count() > 1
            select $"{g.Key.FirstName} {g.Key.LastName}: {g.Count()} active courses";
    }

    public IEnumerable<string> Challenge02_AprilCoursesWithoutFinalGrades()
    {
        return from c in UniversityData.Courses
            where c.StartDate.Year == 2026 && c.StartDate.Month == 4
            join e in UniversityData.Enrollments on c.Id equals e.CourseId
            group e by c.Title into g
            where g.All(e => !e.FinalGrade.HasValue) // Or `g.Count(e => e.FinalGrade.HasValue) == 0`
            select g.Key;
    }

    public IEnumerable<string> Challenge03_LecturersAndAverageGradeAcrossTheirCourses()
    {
        // By putting the enrollments mapping into a 'let' clause, we keep ALL lecturers 
        // as a reporting dimension instead of stripping them in an INNER JOIN / WHERE layer.
        return from l in UniversityData.Lecturers
            let grades = from c in UniversityData.Courses
                where c.LecturerId == l.Id
                join e in UniversityData.Enrollments on c.Id equals e.CourseId
                where e.FinalGrade.HasValue
                select e.FinalGrade.Value
            select $"{l.FirstName} {l.LastName}: {(grades.Any() ? grades.Average().ToString("F2") : "N/A")}";
    }

    public IEnumerable<string> Challenge04_CitiesAndActiveEnrollmentCounts()
    {
        return from s in UniversityData.Students
            join e in UniversityData.Enrollments on s.Id equals e.StudentId
            where e.IsActive
            group e by s.City into g
            orderby g.Count() descending
            select $"{g.Key}: {g.Count()}";
    }
}
