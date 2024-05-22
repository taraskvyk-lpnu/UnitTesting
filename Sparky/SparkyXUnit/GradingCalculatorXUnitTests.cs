using Microsoft.Win32.SafeHandles;
using Sparky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkyXUnitTest
{
    public class GradingCalculatorXUnitTests
    {
        GradingCalculator gradingCalculator;

        public GradingCalculatorXUnitTests()
        {
            gradingCalculator = new GradingCalculator();
        }


        [Theory]
        [InlineData(95, 90, "A")]
        [InlineData(85, 90, "B")]
        [InlineData(65, 90, "C")]
        [InlineData(95, 65, "B")]
        public void GetGrade_ValidInput_ReturnsValidGrades(int score, int attendancePercentage, string expectedResult)
        {
            gradingCalculator.Score = score;
            gradingCalculator.AttendancePercentage = attendancePercentage;

            Assert.Equal(expectedResult, gradingCalculator.GetGrade());
        }

        [Theory]
        [InlineData(95, 55)]
        [InlineData(65, 55)]
        [InlineData(50, 90)]
        public void GetGrade_ValidInput_ReturnsValidGradeF(int score, int attendancePercentage)
        {
            gradingCalculator.Score = score;
            gradingCalculator.AttendancePercentage = attendancePercentage;

            Assert.Multiple(() =>
            {
                Assert.Equal("F", gradingCalculator.GetGrade());
            });
        }

        [Theory]
        [InlineData(95, 90, "A")]
        [InlineData(85, 90, "B")]
        [InlineData(65, 90, "C")]
        [InlineData(95, 65, "B")]
        [InlineData(95, 55, "F")]
        [InlineData(65, 55, "F")]
        [InlineData(50, 90, "F")]
        public void GetGrade_ValidInput_ReturnsValidAllGrades(int score, int attendancePercentage, string expectedResult)
        {
            gradingCalculator.Score = score;
            gradingCalculator.AttendancePercentage = attendancePercentage;

            Assert.Equal(expectedResult, gradingCalculator.GetGrade());
        }

    }
}
