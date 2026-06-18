/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Projections                         Component : Adapters Layer                        *
*  Assembly : Empiria.CashFlow.Projections.dll             Pattern   : Structurer mapper                     *
*  Type     : StructureForEditCashFlowProjectionsMapper    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Maps structured data used for cash flow projections edition.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.Parties;

using Empiria.Financial;
using Empiria.Financial.Projects;

namespace Empiria.CashFlow.Projections.Adapters {

  /// <summary>Maps structured data used for cash flow projections edition.</summary>
  static internal class StructureForEditCashFlowProjectionsMapper {

    static private FixedList<FinancialProject> _allProjects;
    static private FixedList<FinancialAccount> _allAccounts;

    static internal FixedList<StructureForEditCashFlowProjections> Map(FixedList<OrganizationalUnit> list) {

      _allAccounts = FinancialAccount.GetList()
                                     .FindAll(x =>
                                        x.FinancialAccountType.PlaysRole(CashFlowProjection.BASE_ACCOUNT_ROLE) &&
                                        list.Contains(x.OrganizationalUnit)
                                     );


      if (list.Any(x => x.PlaysRole("oficina-promocion"))) {
        _allProjects = FixedList<FinancialProject>.MergeDistinct(_allAccounts.SelectDistinct(x => x.Project),
                                                               FinancialProject.GetList<FinancialProject>()
                                                                               .FindAll(x => x.AllowNewProspectedAccounts));
      } else {
        _allProjects = _allAccounts.SelectDistinct(x => x.Project);
      }

      _allProjects = _allProjects.OrderBy(x => x.ProjectNo)
                                 .ToFixedList();

      return list.Select(x => Map(x))
                 .ToFixedList();
    }


    static private StructureForEditCashFlowProjections Map(OrganizationalUnit orgUnit) {
      var structurer = new Structurer(orgUnit);

      return new StructureForEditCashFlowProjections {
        UID = orgUnit.UID,
        Name = $"{orgUnit.Code} - {orgUnit.Name}",

        Plans = CashFlowPlan.GetList()
                            .MapToNamedEntityList(),

        ProjectionTypes = structurer.GetProjectionCategories()
      };
    }


    private class Structurer {

      private readonly OrganizationalUnit _orgUnit;
      private readonly FixedList<FinancialProject> _projects;
      private readonly FixedList<FinancialAccount> _accounts;

      internal Structurer(OrganizationalUnit orgUnit) {
        _orgUnit = orgUnit;
        _accounts = _allAccounts.FindAll(x => x.OrganizationalUnit.Equals(_orgUnit));

        if (_orgUnit.PlaysRole("oficina-promocion")) {
          _projects = _allProjects.FindAll(x => _accounts.Contains(y => (y.OrganizationalUnit.Equals(_orgUnit) &&
                                                                         y.Project.Equals(x))) ||
                                                                         x.AllowNewProspectedAccounts);
        } else {
          _projects = _allProjects.FindAll(x => _accounts.Contains(y => y.OrganizationalUnit.Equals(_orgUnit) &&
                                                                        y.Project.Equals(x)));
        }
      }


      internal FixedList<ProjectionTypeForEditionDto> GetProjectionCategories() {
        var projectionCategories = CashFlowProjectionCategory.GetList();

        return projectionCategories.Select(x => MapCategory(x))
                                   .ToFixedList();
      }


      private ProjectionTypeForEditionDto MapCategory(CashFlowProjectionCategory projectionCategory) {
        var sources = projectionCategory.OperationSources;

        return new ProjectionTypeForEditionDto {
          UID = projectionCategory.UID,
          Name = projectionCategory.Name,
          ProjectTypes = MapBaseProjectTypes(projectionCategory),
          Sources = sources.MapToNamedEntityList()
        };
      }


      private FixedList<ProjectTypeForEditionDto> MapBaseProjectTypes(CashFlowProjectionCategory projectionCategory) {

        FixedList<FinancialProjectCategory> projectCategories = null;


        projectCategories = _projects.SelectDistinct(x => x.Category);

        return projectCategories.Select(x => MapProjectType(x))
                                .ToFixedList();
      }


      private ProjectTypeForEditionDto MapProjectType(FinancialProjectCategory baseProjectCategory) {
        return new ProjectTypeForEditionDto {
          UID = baseProjectCategory.UID,
          Name = baseProjectCategory.Name,
          Projects = MapProjects(baseProjectCategory)
        };
      }

      private FixedList<ProjectionProjectForEdition> MapProjects(FinancialProjectCategory projectCategory) {

        FixedList<FinancialProject> projects = _projects.OrderBy(x => ((INamedEntity) x).Name)
                                                        .ToFixedList();

        return projects.Select(x => MapFinancialProject(x))
                       .ToFixedList();
      }


      private ProjectionProjectForEdition MapFinancialProject(FinancialProject project) {
        return new ProjectionProjectForEdition {
          UID = project.UID,
          Name = ((INamedEntity) project).Name,
          Accounts = MapProjectAccounts(project)
        };
      }


      private FixedList<NamedEntityDto> MapProjectAccounts(FinancialProject project) {
        var accounts = new List<NamedEntityDto>(24);

        if (project.AllowNewProspectedAccounts) {
          accounts.Add(new NamedEntityDto(Guid.Empty.ToString(), "Crear crédito prospectado"));
        }

        accounts.AddRange(_accounts.FindAll(x => x.Project.Equals(project))
                                   .Sort((x, y) => ((INamedEntity) x).Name.CompareTo(((INamedEntity) y).Name))
                                   .MapToNamedEntityList());

        return accounts.ToFixedList();
      }
    }  // class Structurer

  }  // class StructureForEditCashFlowProjectionsMapper

}  // namespace Empiria.CashFlow.Projections.Adapters
