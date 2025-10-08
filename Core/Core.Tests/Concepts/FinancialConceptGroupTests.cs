/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Concepts                         Component : Test cases                              *
*  Assembly : Empiria.Financial.Core.Tests.dll           Pattern   : Unit tests                              *
*  Type     : FinancialConceptGroupTests                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for FinancialGroup type.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial.Concepts;

namespace Empiria.Tests.Financial.Concepts {

  /// <summary>Unit tests for FinancialGroup type.</summary>
  public class FinancialConceptGroupTests {

    #region Facts

    [Fact]
    public void Should_Get_FinancialConceptGroup_Concepts() {
      var sut = TestsObjects.TryGetObject<FinancialConceptGroup>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut.GetConcepts());
    }


    [Fact]
    public void Should_Get_FinancialConceptGroup_List() {
      var sut = FinancialConceptGroup.GetList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_All_Financial_Concept_Groups() {
      var sut = BaseObject.GetFullList<FinancialConceptGroup>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_FinancialConceptGroup() {
      var sut = FinancialConceptGroup.Empty;

      Assert.NotNull(sut);
      Assert.Equal(-1, sut.Id);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(FinancialConceptGroup.Parse("Empty"), sut);
      Assert.Empty(sut.GetConcepts());
    }

    #endregion Facts

  }  // class FinancialConceptGroupTests

}  // namespace Empiria.Tests.Financial.Concepts
