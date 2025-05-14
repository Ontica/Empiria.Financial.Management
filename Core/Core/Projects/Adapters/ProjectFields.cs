/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Output DTO                            *
*  Type     : ProjectField                                  License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Input field with financial projects data.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Contacts;
using Empiria.Parties;
using Empiria.StateEnums;
using System;

namespace Empiria.Financial.Projects.Adapters {

  /// <summary>Input field with financial projects data.</summary>
  public class ProjectFields {

    public int TypeId {
      get; set;
    } = 0;


    public int StandarAccountId {
      get; set;
    } = 0;


    public int CategoryId {
      get; set;
    } = 0;


    public string PrjNo {
      get; set;
    } = string.Empty;


    public string Name {
      get; set;
    } = string.Empty;


    public string OrganizationUnitUID {
      get; set;
    } = string.Empty;

    public EntityStatus Status {
      get; set;
    } = EntityStatus.Active;

    internal void EnsureValid() {
      Assertion.Require(PrjNo, "Necesito el numero de proyecto.");
      Assertion.Require(Name, "Necesito el nombre de proyecto.");
      Assertion.Require(CategoryId, "Necesito el tipo de proyecto.");
      Assertion.Require(OrganizationUnitUID, "Necesito el area.");
      _ = Party.Parse(OrganizationUnitUID);
    }
  }  // class ProjecFields

}  // namespace Empiria.Financial.Projects.Adapters
