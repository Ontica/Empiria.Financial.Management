/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Value type                              *
*  Type     : BudgetDataInMonths                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Value type that holds budget data with months in columns.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Budgeting.Transactions;

namespace Empiria.Budgeting.Explorer {

  /// <summary>Value type that holds budget data with months in columns.</summary>
  public class BudgetDataInMonths {

    [DataField("BUDGET_ID")]
    public Budget Budget {
      get; private set;
    }


    [DataField("YEAR", ConvertFrom = typeof(long))]
    public int Year {
      get; private set;
    }


    [DataField("BUDGET_ACCT_ID")]
    public BudgetAccount BudgetAccount {
      get; private set;
    }


    [DataField("BALANCE_COLUMN_ID")]
    public BalanceColumn BalanceColumn {
      get; private set;
    }


    [DataField("BALANCE_COLUMN")]
    public string BalanceColumnName {
      get; private set;
    }


    [DataField("ENERO")]
    public decimal Month_01 {
      get; private set;
    }


    [DataField("FEBRERO")]
    public decimal Month_02 {
      get; private set;
    }


    [DataField("MARZO")]
    public decimal Month_03 {
      get; private set;
    }


    [DataField("ABRIL")]
    public decimal Month_04 {
      get; private set;
    }


    [DataField("MAYO")]
    public decimal Month_05 {
      get; private set;
    }


    [DataField("JUNIO")]
    public decimal Month_06 {
      get; private set;
    }


    [DataField("JULIO")]
    public decimal Month_07 {
      get; private set;
    }


    [DataField("AGOSTO")]
    public decimal Month_08 {
      get; private set;
    }


    [DataField("SEPTIEMBRE")]
    public decimal Month_09 {
      get; private set;
    }


    [DataField("OCTUBRE")]
    public decimal Month_10 {
      get; private set;
    }


    [DataField("NOVIEMBRE")]
    public decimal Month_11 {
      get; private set;
    }


    [DataField("DICIEMBRE")]
    public decimal Month_12 {
      get; private set;
    }

  }  // class BudgetDataInMonths

}  // namespace Empiria.Budgeting.Explorer
