/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Mapper                                  *
*  Type     : CashFlowExplorerResultMapper               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps a CashFlowExplorerResult instance to its output DTO.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.Explorer.Adapters {

  /// <summary>Maps a CashFlowExplorerResult instance to its output DTO.</summary>
  static internal class CashFlowExplorerResultMapper {

    static internal DynamicDto<T> Map<T>(DynamicResult<T> result) {
      Assertion.Require(result, nameof(result));

      return new DynamicDto<T> {
        Query = result.Query,
        Columns = result.Columns,
        Entries = result.Entries
      };
    }

  }  // class CashFlowExplorerResultMapper

}  // namespace Empiria.CashFlow.Explorer.Adapters
