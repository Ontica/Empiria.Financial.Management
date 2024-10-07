/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Project Management                                 Component : Adapters Layer                  *
*  Assembly : Empiria.ProjectType.Core.dll                       Pattern   : Output DTO                      *
*  Type     : ProjectTypeDto                                     License   : Please read LICENSE.txt file    *
*                                                                                                            *
*  Summary  : Output DTO for ProjectType instances.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Projects.Adapters {

  public class ProjectTypeDto {

    public string UID {
      get; internal set;
    }


    public string Name {
      get; internal set;
    }


    public string Code {
      get; internal set;
    }


    /*
        public FixedList<ProjectDto> Projects
        {
            get; internal set;
        }
    */

  } // Class ProjectTypeDto

} // namespace Empiria.Projects.Adapters
