/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Fields DTO                              *
*  Type     : PayableFields                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Fields structure used for create and update payable objects.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial;
using Empiria.Parties;

using Empiria.Budgeting;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Fields structure used for create and update payable objects.</summary>
  public class PayableFields {

    #region Properties

    public string PayableTypeUID {
      get; set;
    } = string.Empty;


    public string PayableEntityUID {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string OrganizationalUnitUID {
      get; set;
    } = string.Empty;


    public string PayToUID {
      get; set;
    } = string.Empty;


    public string CurrencyUID {
      get; set;
    } = string.Empty;


    public string BudgetTypeUID {
      get; set;
    } = string.Empty;


    public DateTime DueTime {
      get; set;
    } = ExecutionServer.DateMinValue;


    public DateTime RequestedTime {
      get; set;
    } = ExecutionServer.DateMinValue;

    #endregion Properties

    #region Methods

    virtual internal void EnsureValid() {
     
      Assertion.Require(OrganizationalUnitUID, "Necesito se proporcione el área.");
      _ = OrganizationalUnit.Parse(OrganizationalUnitUID);

      Assertion.Require(PayToUID, "Necesito saber a quien se le realizará el pago.");
      _ = Party.Parse(PayToUID);

      Assertion.Require(CurrencyUID, "Necesito la moneda.");
      _ = Currency.Parse(CurrencyUID);

      Assertion.Require(BudgetTypeUID, "Necesito saber con qué presupuesto se hará el pago.");
      _ = BudgetType.Parse(BudgetTypeUID);
    }


    #endregion Methods

  }  // class PayableFields

}  // namespace Empiria.Payments.Payables.Adapters
