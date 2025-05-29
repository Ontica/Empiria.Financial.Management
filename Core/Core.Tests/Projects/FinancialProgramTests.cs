/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Projects Management                        Component : Test cases                              *
*  Assembly : Empiria.Projects.Core.Tests.dll            Pattern   : Unit tests                              *
*  Type     : FinancialProgramTests                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for FinancialProgram type.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial.Projects;

namespace Empiria.Tests.Financial.Projects {

  /// <summary>Unit tests for FinancialProgram type.</summary>
  public class FinancialProgramTests {

    #region Facts

    [Fact]
    public void Should_Get_All_Programs() {
      var sut = BaseObject.GetFullList<FinancialProgram>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Parse_Empty_Program() {
      var sut = FinancialProgram.Empty;

      Assert.NotNull(sut);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(-1, sut.Id);
      Assert.Equal(FinancialProgramType.Empty, sut.FinancialProgramType);
      Assert.NotNull(sut.Children);
      Assert.Empty(sut.Children);
      Assert.Equal(sut, sut.Parent);
    }


    [Fact]
    public void Should_Parse_All_Programs() {
      var programs = FinancialProgram.GetList();

      foreach (var sut in programs) {
        Assert.NotEmpty(sut.ProgramNo);
        Assert.NotEmpty(sut.Name);
        Assert.NotNull(sut.Parent);
        Assert.NotNull(sut.Children);
        Assert.Equal(sut.Level, (int) sut.FinancialProgramType);
        Assert.NotEmpty(sut.Keywords);
      }
    }

    #endregion Facts

  }  // class FinancialProgramTests

}  // namespace Empiria.Tests.Financial.Projects
