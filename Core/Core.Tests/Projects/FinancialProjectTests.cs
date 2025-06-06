﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Projects Management                        Component : Test cases                              *
*  Assembly : Empiria.Projects.Core.Tests.dll            Pattern   : Unit tests                              *
*  Type     : FinancialProjectTests                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for FinancialProject type.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using System;

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial.Projects;
using Empiria.Financial.Projects.Adapters;
using Empiria.Financial.Projects.Data;

namespace Empiria.Tests.Financial.Projects {

  /// <summary>Unit tests for FinancialProject type.</summary>
  public class FinancialProjectTests {

    #region Facts

    [Fact]
    public void Should_Create_FinancialProject() {

      string name = "Proyecto de prueba";

      var category = TestsObjects.TryGetObject<FinancialProjectCategory>();

      var baseOrgUnit = TestsObjects.TryGetObject<OrganizationalUnit>(x => x.PlaysRole(FinancialProjectRules.PROJECT_MANGER_ROLE));

      var subprogram = TestsObjects.TryGetObject(FinancialProgram.GetList(FinancialProgramType.Subprograma));

      var sut = new FinancialProject(category, baseOrgUnit, subprogram, name);

      Assert.Equal(baseOrgUnit, sut.BaseOrgUnit);
      Assert.Equal(name, sut.Name);

      Assert.Equal(subprogram, sut.Subprogram);
      Assert.Equal(category, sut.Category);
      Assert.True(sut.ProjectNo.Length != 0);
      Assert.Equal(DateTime.Today, sut.StartDate);
      Assert.Equal(ExecutionServer.DateMaxValue, sut.EndDate);
      Assert.Equal(FinancialProject.Empty, sut.Parent);
      Assert.Equal(EntityStatus.Pending, sut.Status);
    }


    [Fact]
    public void Should_Delete_FinancialProject() {
      FinancialProject sut = TestsObjects.TryGetObject<FinancialProject>(x => x.Status == EntityStatus.Pending);

      if (sut == null) {
        return;
      }

      sut.Delete();

      Assert.Equal(EntityStatus.Deleted, sut.Status);
    }


    [Fact]
    public void Should_Parse_All_Projects() {
      var sut = BaseObject.GetFullList<FinancialProject>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Parse_Empty_Project() {
      var sut = FinancialProject.Empty;

      Assert.NotNull(sut);
      Assert.Equal(-1, sut.Id);
    }


    [Fact]
    public void Should_Search_Financial_Projects_By_Keywords() {
      string keywords = "potable agua";

      FixedList<FinancialProject> sut = FinancialProjectDataService.SearchProjects(keywords);

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Search_Financial_Projects_By_Query() {
      var query = new FinancialProjectQuery {
        Keywords = "potable agua",
        Status = EntityStatus.Active
      };

      string filter = query.MapToFilterString();
      string sort = query.MapToSortString();

      FixedList<FinancialProject> sut = FinancialProjectDataService.SearchProjects(filter, sort);

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Update_FinancialProject() {
      FinancialProject sut = TestsObjects.TryGetObject<FinancialProject>();

      if (sut == null) {
        return;
      }

      var fields = new FinancialProjectFields {
        ProjectNo = "0920",
        Name = "Nuevo nombre",
      };

      var unchangedFields = new FinancialProjectFields {
        SubprogramUID = sut.Subprogram.UID,
        ProjectTypeCategoryUID = sut.Category.UID,
        BaseOrgUnitUID = sut.BaseOrgUnit.UID,
      };

      sut.Update(fields);

      Assert.Equal(fields.ProjectNo, sut.ProjectNo);
      Assert.Equal(fields.Name, sut.Name);
      Assert.Equal(unchangedFields.SubprogramUID, sut.Subprogram.UID);
      Assert.Equal(unchangedFields.ProjectTypeCategoryUID, sut.Category.UID);
      Assert.Equal(unchangedFields.BaseOrgUnitUID, sut.BaseOrgUnit.UID);
    }

    #endregion Facts

  }  // class FinancialProjectTests

}  // namespace Empiria.Tests.Financial.Projects
