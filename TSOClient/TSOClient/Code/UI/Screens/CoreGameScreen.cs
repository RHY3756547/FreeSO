﻿/*The contents of this file are subject to the Mozilla Public License Version 1.1
(the "License"); you may not use this file except in compliance with the
License. You may obtain a copy of the License at http://www.mozilla.org/MPL/

Software distributed under the License is distributed on an "AS IS" basis,
WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
the specific language governing rights and limitations under the License.

The Original Code is the TSOClient.

The Initial Developer of the Original Code is
ddfczm. All Rights Reserved.

Contributor(s): ______________________________________.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSOClient.Code.UI.Framework;
using TSOClient.Code.UI.Panels;
using TSOClient.Code.UI.Model;
using TSOClient.ThreeD;
using TSOClient.Code.Rendering.City;
using Microsoft.Xna.Framework;
using TSOClient.Code.Utils;

namespace TSOClient.Code.UI.Screens
{
    public class CoreGameScreen : GameScreen
    {
        private UIUCP ucp;
        private UIGizmo gizmo;
        private Terrain CityRenderer;

        public CoreGameScreen()
        {
            /** City Scene **/
            ListenForMouse(new Rectangle(0, 0, ScreenWidth, ScreenHeight), new UIMouseEvent(MouseHandler));

            CityRenderer = new Terrain(); // The Terrain class implements the ThreeDAbstract interface so that it can be treated as a scene but manage its own drawing and updates.

            String city = "Queen Margret's";
            if (PlayerAccount.CurrentlyActiveSim != null)
                city = PlayerAccount.CurrentlyActiveSim.ResidingCity.Name;

            CityRenderer.m_GraphicsDevice = GameFacade.GraphicsDevice;

            CityRenderer.Initialize(city, new CityDataRetriever());
            CityRenderer.RegenData = true;
            
            CityRenderer.LoadContent(GameFacade.GraphicsDevice);

            /**
            * Music
            */
            var tracks = new string[]{
                GlobalSettings.Default.StartupPath + "\\music\\modes\\map\\tsobuild1.mp3",
                GlobalSettings.Default.StartupPath + "\\music\\modes\\map\\tsobuild3.mp3",
                GlobalSettings.Default.StartupPath + "\\music\\modes\\map\\tsomap2_v2.mp3",
                GlobalSettings.Default.StartupPath + "\\music\\modes\\map\\tsomap3.mp3",
                GlobalSettings.Default.StartupPath + "\\music\\modes\\map\\tsomap4_v1.mp3"
            };
            PlayBackgroundMusic(
                tracks
            );

            //Shadow configuration. Very Low quality res: 512, Low quality: 1024, High quality: 2048
            CityRenderer.ShadowsEnabled = true;
            CityRenderer.ShadowRes = 2048;

            ucp = new UIUCP();
            ucp.Y = ScreenHeight - 210;
            this.Add(ucp);

            gizmo = new UIGizmo();
            gizmo.X = ScreenWidth - 500;
            gizmo.Y = ScreenHeight - 300;
            this.Add(gizmo);

            GameFacade.Scenes.AddScene((ThreeDAbstract)CityRenderer);
        }

        private void MouseHandler(UIMouseEventType type, UpdateState state)
        {
            //todo: change handler to game engine when in simulation mode.

            CityRenderer.UIMouseEvent(type.ToString()); //all the city renderer needs are events telling it if the mouse is over it or not.
            //if the mouse is over it, the city renderer will handle the rest.
        }
    }
}
