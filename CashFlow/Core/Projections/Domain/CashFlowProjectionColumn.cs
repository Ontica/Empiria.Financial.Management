/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Information Holder                      *
*  Type     : CashFlowProjectionColumn                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a column balance that holds inflows or outflows for a cash flow projection entry.   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.Projections {

  /// <summary>Represents a column balance that holds inflows or outflows
  /// for a cash flow projection entry.</summary>
  public class CashFlowProjectionColumn : CommonStorage, INamedEntity {

    #region Constructors and parsers

    static public CashFlowProjectionColumn Parse(int id) => ParseId<CashFlowProjectionColumn>(id);

    static public CashFlowProjectionColumn Parse(string uid) => ParseKey<CashFlowProjectionColumn>(uid);

    static public CashFlowProjectionColumn Empty => ParseEmpty<CashFlowProjectionColumn>();

    #endregion Constructors and parsers

  }  // class CashFlowProjectionColumn

}  // namespace Empiria.CashFlow.Projections
