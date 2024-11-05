/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Query DTO                               *
*  Type     : BudgetExplorerQuery                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to explore budget information.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Explorer.Adapters {

  /// <summary>Input query DTO used to explore budget information.</summary>
  public class BudgetExplorerQuery {

    public string BudgetUID {
      get; set;
    } = string.Empty;


    public string[] GroupBy {
      get; set;
    } = new string[0];


    public BudgetSegmentQuery[] FilteredBy {
      get; set;
    } = new BudgetSegmentQuery[0];

  }  // class BudgetExplorerQuery



  /// <summary>Input query DTO used to filter budget segments.</summary>
  public class BudgetSegmentQuery {

    [Newtonsoft.Json.JsonProperty(PropertyName = "SegmentUID")]
    public string SegmentTypeUID {
      get; set;
    } = string.Empty;


    public string[] SegmentItems {
      get; set;
    } = new string[0];

  }  // class BudgetSegmentQuery

}  // namespace Empiria.Budgeting.Explorer.Adapters
