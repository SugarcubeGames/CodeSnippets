using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomEditorWindow : EditorWindow{

    /***********************
     * Top toolbar variables
     * *********************/
    /// <summary>
    /// Texture for the New Room button.
    /// </summary>
    private Texture buttonNew;
    /// <summary>
    /// Texture for the Save Room button.
    /// </summary>
    private Texture buttonSave;
    /// <summary>
    /// Texture for the Open Room buttons.
    /// </summary>
    private Texture buttonOpen;


    /// <summary>
    /// Is there currently a room open for editing?
    /// </summary>
    //private bool isEditingRoom = false;
    /// <summary>
    /// Has the room been modified?  (Tracked for indicating save need)
    /// </summary>
    private bool hasModifiedRoom = false;
    /// <summary>
    /// Reference to the scriptable object which contains all the editor icons
    /// </summary>
    private RoomEditorIconRef iconRef;

    //Scrollview variabls
    /// <summary>
    /// Scrollview position
    /// </summary>
    private Vector2 svPos;
    /// <summary>
    /// Scrollview Rect.
    /// </summary>
    private Rect svRect;
    /// <summary>
    /// Grid Spacing in the Scrollview
    /// </summary>
    private int gridSpaceing = 32;

    /// <summary>
    /// The current room being edited
    /// </summary>
    private Room room;
    /// <summary>
    /// The room's current name
    /// </summary>
    private string roomName;
    /// <summary>
    /// The room's current type
    /// </summary>
    private LEVELTYPES roomType;
    /// <summary>
    /// The room's entrance location
    /// </summary>
    private ROOMENTRYEXITLOCATIONS roomEntrance;
    /// <summary>
    /// The room's exit location
    /// </summary>
    private ROOMENTRYEXITLOCATIONS roomExit;

    [MenuItem("Infinivania/Room Editor")]
    static void Init()
    {
        //Get existing open window or, if none, create a new one
        RoomEditorWindow window = (RoomEditorWindow)
            EditorWindow.GetWindow(typeof(RoomEditorWindow));
        window.Show();

        window.minSize = new Vector2(100.0f, 100.0f);

        window.init();
    }

    /// <summary>
    /// Initialize all window variables
    /// </summary>
    public void init()
    {
        //Instantiate the icon ref
        iconRef = RoomEditorIconRef.CreateInstance<RoomEditorIconRef>();
        buttonNew = iconRef.editorButtonNewRoom;
        buttonOpen = iconRef.editorButtonOpenRoom;
        buttonSave = iconRef.editorButtonSaveRoom;
    }

    /*********************************************************************************
     * NOTE:
     * To get the last rectnagle used to daw a particular area:
     * 
     * rect lastRect;
     * 
     * if(Event.current.type == EventType.Repaint(
     * {
     *      lastRect = GUILayoutUtility.GetLastRect();
     * }
     * 
     * You have to do this in the Repaint event, because int he layout event
     * the rectangle's values are all 0.
     * *******************************************************************************/
    public void OnGUI()
    {

        /************************************************************
         * Top toolbar: New, Save, Open
         * **********************************************************/
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        
        //EditorGUILayout.LabelField("Test");
        if (GUILayout.Button(buttonNew, EditorStyles.label, GUILayout.Width(32), GUILayout.Height(32)))
        {
            createNewRoom();
        }

        if (GUILayout.Button(buttonOpen, EditorStyles.label, GUILayout.Width(32), GUILayout.Height(32)))
        {
            openRoom();
        }

        if (GUILayout.Button(buttonSave, EditorStyles.label, GUILayout.Width(32), GUILayout.Height(32)))
        {
            saveRoom();
        }
        EditorGUILayout.EndHorizontal();

        //If no room is loaded, don't draw anyhitng else
        if (room == null) return;

        /*************************************************************
         * Side bar: Name, Type, and tile selection dropdown (ground, doors, enemies, etc.)
         * ***********************************************************/
         //Horizontal encapsulates the sidebar and the scrollview
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(100), GUILayout.ExpandHeight(true));

        roomName = EditorGUILayout.TextField(roomName);
        GUIUtils.DrawCenteredText("Castle Area:");
        roomType = (LEVELTYPES)EditorGUILayout.EnumPopup("", roomType);
        GUIUtils.DrawCenteredText("Entrance Location:");
        roomEntrance = (ROOMENTRYEXITLOCATIONS)EditorGUILayout.EnumPopup("", roomEntrance);
        GUIUtils.DrawCenteredText("Exit Location:");
        roomExit = (ROOMENTRYEXITLOCATIONS)EditorGUILayout.EnumPopup("", roomExit);

        //Draw divider
        GUIUtils.DrawHorizontalDivider();

        EditorGUILayout.EndVertical();

        /*****************************************************************
         * Scrollview Which shows the level grid.  0,0 starts at the bottom
         * left corner.  
         * ***************************************************************/

        //Scrollview plus bottom toolbar
        EditorGUILayout.BeginVertical();

        //Level editor scrollview
        EditorGUILayout.BeginScrollView(svPos, EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

        if (svRect.Contains(Event.current.mousePosition))
        {
            //Scroll the view by center-clicking
            if (Event.current.button == 2 && Event.current.type == EventType.MouseDrag)
            {
                svPos += Event.current.delta;
                Event.current.Use();
            }

            //
        }

        EditorGUILayout.EndScrollView();
        //End level editor scrollview

        if (Event.current.type == EventType.Layout)
        {
            //Debug.Log("6 svRect:  " + svRect);

        } else if (Event.current.type == EventType.Repaint)
        {
            //Debug.Log("6 Repaint:  " + GUILayoutUtility.GetLastRect().ToString())
            svRect = GUILayoutUtility.GetLastRect();

            Handles.color = Color.black;

            //Handles.DrawLine(new Vector3(svRect.x, svRect.y, 1.0f), new Vector3(svRect.x + svRect.width, svRect.y + svRect.height, 1.0f));

            //svPos.x%64 should start us at the next porper grid line position relative to the left edge of the scroll view.
            //Horizontal grid lines
            for (int i = (int)(svPos.x % gridSpaceing) + (int)svRect.x; i<(svRect.x + svRect.width); i+=gridSpaceing)
            {
                if (i < svRect.x) continue;

                if(calcGridNumber(i, true) == 0)
                {
                    Handles.color = Color.blue;
                    Handles.DrawLine(new Vector3(i + 1, svRect.y, 1.0f), new Vector3(i + 1, svRect.y + svRect.height, 1.0f));
                    Handles.DrawLine(new Vector3(i - 1, svRect.y, 1.0f), new Vector3(i - 1, svRect.y + svRect.height, 1.0f));
                    Handles.DrawLine(new Vector3(i, svRect.y, 1.0f), new Vector3(i, svRect.y + svRect.height, 1.0f));
                    Handles.color = Color.black;
                } else if (calcGridNumber(i, true)%16 == 0)
                {
                    Handles.color = Color.green;
                    Handles.DrawLine(new Vector3(i + 1, svRect.y, 1.0f), new Vector3(i + 1, svRect.y + svRect.height, 1.0f));
                    Handles.DrawLine(new Vector3(i, svRect.y, 1.0f), new Vector3(i, svRect.y + svRect.height, 1.0f));
                    Handles.color = Color.black;
                } else
                {
                    Handles.DrawLine(new Vector3(i, svRect.y, 1.0f), new Vector3(i, svRect.y + svRect.height, 1.0f));
                }
            }

            //Vertical grid lines
            for(int i = (int)(svPos.y%gridSpaceing) + (int)svRect.y; i<(svRect.y + svRect.height); i += gridSpaceing)
            {
                if (i < svRect.y) continue;
                if (calcGridNumber(i, false) == 0)
                {
                    Handles.color = Color.blue;
                    Handles.DrawLine(new Vector3(svRect.x, i + 1, 1.0f), new Vector3(svRect.x + svRect.width, i + 1, 1.0f));
                    Handles.DrawLine(new Vector3(svRect.x, i - 1, 1.0f), new Vector3(svRect.x + svRect.width, i - 1, 1.0f));
                    Handles.DrawLine(new Vector3(svRect.x, i, 1.0f), new Vector3(svRect.x + svRect.width, i, 1.0f));
                    Handles.color = Color.black;
                } else if (calcGridNumber(i, false) %11 == 0)
                {
                    Handles.color = Color.green;
                    Handles.DrawLine(new Vector3(svRect.x, i + 1, 1.0f), new Vector3(svRect.x + svRect.width, i + 1, 1.0f));
                    Handles.DrawLine(new Vector3(svRect.x, i, 1.0f), new Vector3(svRect.x + svRect.width, i, 1.0f));
                    Handles.color = Color.black;
                } else
                {
                    Handles.DrawLine(new Vector3(svRect.x, i, 1.0f), new Vector3(svRect.x + svRect.width, i, 1.0f));
                }
            }

            /*
            Handles.color = Color.blue;
            Handles.DrawLine(new Vector3(svRect.x, svRect.y, 1.0f), Event.current.mousePosition);
            */
        }

        //Bottom toolbar
        EditorGUILayout.BeginHorizontal(GUILayout.Height(64));

        //Active tile display
        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(64), GUILayout.ExpandHeight(true));
        EditorGUILayout.LabelField("Active Tile", GUILayout.Width(64));
        EditorGUILayout.EndVertical();
        //End Active Tile Display

        //Output
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Height(64));

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Map Pos: " + calcMousePositionToGridLocation(Event.current.mousePosition));

        //Debug output
        /*
        EditorGUILayout.LabelField(svRect.ToString());
        if (svRect.Contains(Event.current.mousePosition)){
            EditorGUILayout.LabelField(Event.current.mousePosition.ToString() + "   " + calcRelativeMouseView(Event.current.mousePosition).ToString() + "   " + svPos.ToString()
                    + "   " + calcMousePositionToGridLocation(Event.current.mousePosition));
        } else
        {
            EditorGUILayout.LabelField("Not in scrollview");
        }
        */

        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
        //End Output

        //Reset Button - returns the scrollview to its starting position
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Width(64),GUILayout.ExpandHeight(true));
        if (GUILayout.Button("Reset"))
        {
            resetScrollViewPos();
        }
        EditorGUILayout.EndHorizontal();
        //End reset button

        EditorGUILayout.EndHorizontal();
        //End Bottom Toolbar

        EditorGUILayout.EndVertical();
        //End scrollview and toolbar group

        EditorGUILayout.EndHorizontal();

        //At the end of the GUI, check for changes to variables
        if (roomName != room.Name || roomType != room.RoomType ||
            roomEntrance != room.RoomEntrance || roomExit != room.RoomExit)
        {
            hasModifiedRoom = true;
        }
    }


    /*********************************************************************
     * Scrollview positioning methods
     * *******************************************************************/
    //Convert the current mouse position its position relative to the scrollview
    private Vector2 calcRelativeMouseView(Vector3 mPos)
    {
        return new Vector2(mPos.x - svRect.x, mPos.y - svRect.y);
    }

    private void resetScrollViewPos()
    {
        //Reset scroll view to centered
        svPos = new Vector2(gridSpaceing/2, svRect.height - gridSpaceing/2);
    }


    private Vector2 calcMousePositionToGridLocation(Vector3 mPos)
    {
        Vector3 svRelPos = calcRelativeMouseView(mPos);
        //return new Vector2(Mathf.FloorToInt((svRelPos.x - svPos.x)  / gridSpaceing), Mathf.FloorToInt((svRelPos.y - svPos.y) / gridSpaceing));
        
        //We want to invert the Y coordinate system so that anyhting above the line is positive, and anything below it is negative
        return new Vector2(Mathf.FloorToInt((svRelPos.x - svPos.x)  / gridSpaceing), -Mathf.FloorToInt((svRelPos.y - svPos.y) / gridSpaceing)-1);
    }

    private int calcGridNumber(int i, bool calcX)
    {
        if (calcX)
        {
            return Mathf.FloorToInt((i - svRect.x - svPos.x)/gridSpaceing);
        } else
        {
            return Mathf.FloorToInt((i - svRect.y - svPos.y) / gridSpaceing);
        }
    }


    /*********************************************************************
     * Create, Save, and Open methods
     * *******************************************************************/
    //Create a new room, and set it as the active room
    private void createNewRoom()
    {
        //Debug.Log("Creating new Room");

        if (hasModifiedRoom)
        {
            //Ask about saving

            //reset hasModifiedroom marker
            hasModifiedRoom = false;
        }

        //Get the location we want to save this room to
        //string pathToCreate = EditorUtility.OpenFolderPanel("Where to create new Room?", "", "");

        room = ScriptableObject.CreateInstance<Room>();
        SetEditorRoomData(room);

        //We will not save this new room at the moment, that will be handled when
        //we actually hit save.  Mark the room as modified so it can be saved
        hasModifiedRoom = true;
    }

    private void saveRoom()
    {
        //If there is no room loaded, return to prevent null errors, or if there have been
        //no modifications, there's no reason to save.
        if(room == null || !hasModifiedRoom) return;

        //If the room's name has not been defined, show a dialog to that affect and cancel
        if (room.Name == null || room.Name == "")
        {
            EditorUtility.DisplayDialog("ERROR:", "Please name the room before saving.", "Okay");
            return;
        }

        UpdateRoomData();

        //If this room does not save a save path, or has not yet been created, we need to create it
        if (room.AssetSavePath == "")
        {
            //string pathToSave = EditorUtility.OpenFolderPanel("Where to save this room?", "", "");
            string pathToSave = EditorUtility.SaveFilePanel("Save room to:", "", "", "");

            //Reduce the returned path down to a realtive application path
            if (pathToSave.StartsWith(Application.dataPath))
            {
                pathToSave = "Assets" + pathToSave.Substring(Application.dataPath.Length);
            }

            //Assign the asset save path
            room.setAssetSavePath(pathToSave);

            //AssetDatabase.CreateAsset(room, pathToSave + "/" + room.Name + ".asset");
            AssetDatabase.CreateAsset(room, pathToSave + ".asset");
            AssetDatabase.SaveAssets();
        } else
        {
            AssetDatabase.SaveAssets();
        }

        hasModifiedRoom = false;

        //Set the room to dirty so the assets retains changes between unity instances
        EditorUtility.SetDirty(room);
        
    }

    private void openRoom()
    {
        //Debug.Log("Opening Room");
        string pathToRoom = EditorUtility.OpenFilePanel("Open Room:", "", "");
        if (pathToRoom.StartsWith(Application.dataPath))
        {
            string relPath = "Assets" + pathToRoom.Substring(Application.dataPath.Length);
            room = AssetDatabase.LoadAssetAtPath(relPath, typeof(Room)) as Room;

        }

        //Set all of the temp variables to match the newly-loaded room
        SetEditorRoomData(room);
        resetScrollViewPos();
    }

    /*******************************************************
     * Methods to update room data
     * *****************************************************/
     private void UpdateRoomData()
    {
        room.setName(roomName);
        room.setRoomType(roomType);
        room.setRoomEntrance(roomEntrance);
        room.setRoomExit(roomExit);
    }

    private void SetEditorRoomData(Room r)
    {
        roomName = r.Name;
        roomType = r.RoomType;
        roomEntrance = r.RoomEntrance;
        roomExit = r.RoomExit;
    }

}
