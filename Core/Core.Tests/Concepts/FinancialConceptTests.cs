/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Concepts                         Component : Test cases                              *
*  Assembly : Empiria.Financial.Core.Tests.dll           Pattern   : Unit tests                              *
*  Type     : FinancialConceptTests                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for FinancialConcept type.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial.Concepts;

namespace Empiria.Tests.Financial.Concepts {

  /// <summary>Unit tests for FinancialConcept type.</summary>
  public class FinancialConceptTests {

    #region Facts

    [Fact]
    public void Should_Parse_All_Financial_Concepts() {
      var list = BaseObject.GetFullList<FinancialConcept>();

      foreach (var sut in list) {
        Assert.NotNull(sut);
        Assert.NotNull(sut.Group);
        Assert.NotNull(sut.Parent);
        Assert.NotNull(sut.GetAllChildren());
        Assert.NotNull(sut.GetChildren());
      }
    }


    [Fact]
    public void Should_Read_All_Financial_Concepts() {
      var sut = BaseObject.GetFullList<FinancialConcept>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_FinancialConcept() {
      var sut = FinancialConcept.Empty;

      Assert.NotNull(sut);
      Assert.Equal(-1, sut.Id);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(FinancialConcept.Parse("Empty"), sut);
      Assert.Equal(string.Empty, sut.ConceptNo);
      Assert.Equal(FinancialConceptGroup.Empty, sut.Group);
      Assert.Equal(sut.Parent, sut);
    }

    #endregion Facts

  }  // class FinancialConceptTests

}  // namespace Empiria.Tests.Financial.Concepts
