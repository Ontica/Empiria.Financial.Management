/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetAccountSegmentMapper                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for budget account segments.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;

namespace Empiria.Budgeting.Adapters {

  /// <summary>Mapping methods for budget account segments.</summary>
  static internal class BudgetAccountSegmentMapper {

    static internal FixedList<BudgetAccountSegmentDto> Map(FixedList<StandardAccount> stdAccounts) {
      return stdAccounts.Select(x => Map(x)).ToFixedList();
    }


    static private BudgetAccountSegmentDto Map(StandardAccount stdAccount) {
      BudgetAccountSegmentDto dto = MapWithoutStructure(stdAccount);

      if (!stdAccount.Parent.IsEmptyInstance) {
        dto.Parent = MapWithoutStructure(stdAccount.Parent);
      }

      dto.Children = stdAccount.GetChildren()
                               .Select(x => MapWithoutStructure(x))
                               .ToFixedList();
      return dto;
    }

    static private BudgetAccountSegmentDto MapWithoutStructure(StandardAccount stdAccount) {
      return new BudgetAccountSegmentDto {
        UID = stdAccount.UID,
        Code = stdAccount.StdAcctNo,
        Name = stdAccount.Name,
        Description = stdAccount.Description,
        Type = BudgetSegmentTypesMapper.MapWithoutStructure(stdAccount.Category)
      };
    }

  }  // class BudgetAccountSegmentMapper

}  // namespace Empiria.Budgeting.Adapters
