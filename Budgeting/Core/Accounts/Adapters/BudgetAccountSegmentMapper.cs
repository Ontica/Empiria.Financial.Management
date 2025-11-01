/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetAccountSegmentMapper                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for budget account segments.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Adapters {

  /// <summary>Mapping methods for budget account segments.</summary>
  static internal class BudgetAccountSegmentMapper {

    static internal FixedList<BudgetAccountSegmentDto> Map(FixedList<FormerBudgetAcctSegment> accountSegments) {
      return accountSegments.Select(x => Map(x)).ToFixedList();
    }

    static private BudgetAccountSegmentDto Map(FormerBudgetAcctSegment accountSegment) {
      BudgetAccountSegmentDto dto = MapWithoutStructure(accountSegment);

      if (accountSegment.HasParent) {
        dto.Parent = MapWithoutStructure(accountSegment.Parent);
      }

      dto.Children = accountSegment.Children.Select(x => MapWithoutStructure(x))
                                         .ToFixedList();

      return dto;
    }

    static private BudgetAccountSegmentDto MapWithoutStructure(FormerBudgetAcctSegment accountSegment) {
      return new BudgetAccountSegmentDto {
        UID = accountSegment.UID,
        Code = accountSegment.Code,
        Name = accountSegment.Name,
        Description = accountSegment.Description,
        Type = BudgetSegmentTypesMapper.MapWithoutStructure(accountSegment.BudgetSegmentType)
      };
    }

  }  // class BudgetAccountSegmentMapper

}  // namespace Empiria.Budgeting.Adapters
