/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Test cases                              *
*  Assembly : Empiria.CashFlow.Core.Tests.dll            Pattern   : Unit tests                              *
*  Type     : CashFlowProjectionEntryTests               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for CashFlowProjectionEntry instances.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.StateEnums;

using Empiria.Financial;

using Empiria.CashFlow.Projections;

namespace Empiria.Tests.CashFlow.Projections {

  /// <summary>Unit tests for CashFlowProjectionEntry type.</summary>
  public class CashFlowProjectionEntryTests {

    #region Facts

    [Fact]
    public void Should_Create_CashFlowProjectionEntry() {

      TestsCommonMethods.Authenticate();

      var projection = TestsObjects.TryGetObject<CashFlowProjection>(
                                      x => x.Rules.CanUpdate && x.AvailableCashFlowAccounts().Count != 0
                                    );

      var projectionColumn = TestsObjects.TryGetObject<CashFlowProjectionColumn>();

      FinancialAccount account = TestsObjects.TryGetObject(projection.AvailableCashFlowAccounts());

      var fields = new CashFlowProjectionEntryFields() {
        ProjectionColumnUID = projectionColumn.UID,
        CashFlowAccountUID = account.UID,
        Year = 2026,
        Month = 1,
        Amount = 12345678.97m,
      };

      int count = projection.Entries.Count;

      CashFlowProjectionEntry sut = projection.AddEntry(fields);

      Assert.Equal(account, sut.CashFlowAccount);
      Assert.Equal(projection, sut.Projection);
      Assert.Equal(account, sut.CashFlowAccount);
      Assert.Equal(fields.ProjectionColumnUID, sut.ProjectionColumn.UID);
      Assert.Equal(fields.CashFlowAccountUID, sut.CashFlowAccount.UID);
      Assert.Equal(fields.Year, sut.Year);
      Assert.Equal(fields.Month, sut.Month);
      Assert.Equal(fields.Amount, sut.Amount);
      Assert.Equal(count + 1, sut.Projection.Entries.Count);
      Assert.Empty(sut.Description);
      Assert.Empty(sut.Justification);
    }


    [Fact]
    public void Should_Delete_CashFlowProjectionEntry() {

      TestsCommonMethods.Authenticate();

      var projection = TestsObjects.TryGetObject<CashFlowProjection>(
                          x => x.Rules.CanUpdate && x.Entries.Count != 0
                        );

      CashFlowProjectionEntry sut = TestsObjects.TryGetObject(projection.Entries);

      int count = projection.Entries.Count;

      projection.RemoveEntry(sut);


      Assert.Equal(TransactionStatus.Deleted, sut.Status);
      Assert.Equal(count - 1, sut.Projection.Entries.Count);
    }


    [Fact]
    public void Should_Get_All_CashFlow_Projection_Entries() {
      var sut = BaseObject.GetFullList<CashFlowProjectionEntry>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Get_Empty_CashFlowProjectionEntry() {
      var sut = CashFlowProjectionEntry.Empty;

      Assert.NotNull(sut);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(CashFlowProjectionEntry.Parse("Empty"), sut);
    }


    [Fact]
    public void Should_Parse_All_CashFlow_Projection_Entries() {
      var list = BaseObject.GetFullList<CashFlowProjectionEntry>();

      foreach (var sut in list) {
        Assert.NotNull(sut.Projection);
        Assert.NotNull(sut.CashFlowAccount);
        Assert.NotNull(sut.ProjectionColumn);
        Assert.NotNull(sut.Product);
        Assert.NotNull(sut.ProductUnit);
        Assert.NotNull(sut.Description);
        Assert.NotNull(sut.Justification);
        Assert.NotNull(sut.Currency);
        Assert.NotNull(sut.ExtensionData);
        Assert.NotEmpty(sut.Keywords);
        Assert.NotEmpty(sut.Tags);
        Assert.NotNull(sut.PostedBy);
        Assert.NotNull(sut.Tags);
      }
    }


    [Fact]
    public void Should_Update_CashFlowProjectionEntry() {

      TestsCommonMethods.Authenticate();

      var projection = TestsObjects.TryGetObject<CashFlowProjection>(
                                x => x.Rules.CanUpdate && x.Entries.Count != 0
                              );

      CashFlowProjectionEntry sut = TestsObjects.TryGetObject(projection.Entries);

      var fields = new CashFlowProjectionEntryFields {
        ExchangeRate = 1.431278m,
        Justification = "Updated justification",
        Description = "Updated description"
      };

      int count = projection.Entries.Count;

      projection.UpdateEntry(sut, fields);

      Assert.Equal(fields.ExchangeRate, sut.ExchangeRate);
      Assert.Equal(fields.Description, sut.Description);
      Assert.Equal(fields.Justification, sut.Justification);
      Assert.Equal(count, sut.Projection.Entries.Count);
    }

    #endregion Facts

  }  // class CashFlowProjectionEntryTests

}  // namespace Empiria.Tests.CashFlow.Projections
