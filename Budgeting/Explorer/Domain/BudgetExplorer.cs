/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Builder                                 *
*  Type     : BudgetExplorer                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Retrives budget information bases on a query returning a dynamic result data structure.        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.DynamicData;

using Empiria.Budgeting.Explorer.Adapters;
using Empiria.Budgeting.Explorer.Data;

namespace Empiria.Budgeting.Explorer {

  /// <summary>Retrives budget information bases on a query returning a dynamic result data structure.</summary>
  internal class BudgetExplorer {

    private readonly BudgetExplorerQuery _query;

    internal BudgetExplorer(BudgetExplorerQuery query) {
      Assertion.Require(query, nameof(query));

      _query = query;
    }

    internal BudgetExplorerResult Execute() {
      return new BudgetExplorerResult {
        Query = _query,
        Columns = BuildColumns(),
        Entries = BudgetExplorerDataService.GetBugetDataInMultipleColumns()
      };
    }

    private FixedList<DataTableColumn> BuildColumns() {
      return new List<DataTableColumn> {
        new DataTableColumn("budgetAccount", "Cuenta", "text"),
        new DataTableColumn("year", "Año", "text"),
        new DataTableColumn("month", "Mes", "text"),
        new DataTableColumn("currency", "Mon", "text"),
        new DataTableColumn("planned", "Planeado", "decimal"),
        new DataTableColumn("expanded", "Ampliaciones", "decimal"),
        new DataTableColumn("reduced", "Reducciones", "decimal"),
        new DataTableColumn("modified", "Modificado", "decimal"),
        new DataTableColumn("modified", "Modificado", "decimal"),
        new DataTableColumn("available", "Disponible", "decimal"),
        new DataTableColumn("commited", "Comprometido", "decimal"),
        new DataTableColumn("toPay", "Por pagar", "decimal"),
        new DataTableColumn("excercised", "Ejercido", "decimal"),
        new DataTableColumn("toExercise", "Por ejercer", "decimal")
      }.ToFixedList();

    }
  }  // class BudgetExplorer

}  // namespace Empiria.Budgeting.Explorer
