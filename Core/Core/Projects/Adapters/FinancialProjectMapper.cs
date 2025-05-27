/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Management.Core.dll      Pattern   : Mapping class                           *
*  Type     : FinancialProjectMapper                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for financial projects.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History;

using Empiria.StateEnums;

using Empiria.Financial.Adapters;

namespace Empiria.Financial.Projects.Adapters {

  /// <summary> Mapping methods for financial projects.</summary>
  static public class FinancialProjectMapper {

    #region Mappers

    static public FinancialProjectHolderDto Map(FinancialProject project) {
      var accounts = project.Accounts.FindAll(x => x.FinancialAccountType.Id == 3245);

      return new FinancialProjectHolderDto {
        Project = MapProject(project),
        Accounts = FinancialAccountMapper.MapToDescriptor(accounts),
        Documents = DocumentServices.GetAllEntityDocuments(project),
        History = HistoryServices.GetEntityHistory(project),
        Actions = MapActions(project.Rules)
      };
    }


    static public FixedList<FinancialProjectDescriptor> Map(FixedList<FinancialProject> project) {
      return project.Select(x => MapToDescriptor(x))
                    .ToFixedList();
    }

    #endregion Mappers

    #region Helpers

    static private BaseActions MapActions(FinancialProjectRules rules) {
      return new BaseActions {
        CanDelete = rules.CanDelete,
        CanEditDocuments = rules.CanEditDocuments,
        CanUpdate = rules.CanUpdate
      };
    }

    static private FinancialProjectDto MapProject(FinancialProject project) {
      return new FinancialProjectDto {
        UID = project.UID,
        Category = project.Category.MapToNamedEntity(),
        Program = project.Program.MapToNamedEntity(),
        Subprogram = project.Subprogram.MapToNamedEntity(),
        BaseOrgUnit = project.BaseOrgUnit.MapToNamedEntity(),
        ProjectNo = project.ProjectNo,
        Name = project.Name,
        Assignee = project.Assignee.MapToNamedEntity(),
        StartDate = project.StartDate,
        EndDate = project.EndDate,
        ParentProject = project.Parent.MapToNamedEntity(),
        Status = project.Status.MapToDto(),
      };
    }

    static private FinancialProjectDescriptor MapToDescriptor(FinancialProject project) {
      return new FinancialProjectDescriptor {
        UID = project.UID,
        CategoryName = project.Category.Name,
        ProgramName = project.Program.Name,
        SubprogramName = project.Subprogram.Name,
        BaseOrgUnitName = project.BaseOrgUnit.Name,
        ProjectNo = project.ProjectNo,
        Name = project.Name,
        AssigneeName = project.Assignee.Name,
        StartDate = project.StartDate,
        EndDate = project.EndDate,
        StatusName = project.Status.GetName(),
      };
    }

    #endregion Helpers

  }  // class FinancialProjectMapper

}  // namespace Empiria.Financial.Projects.Adapters
