<?php
//Recycled first year work from Databases and the web module.
//DATABASE GENERIC ACCESS-----------------------------------------------------------------------------------------------------------

//Declaring database and db variable
$db = NULL;
$dbfilename = '..\XLN-bot\BlazorTest\Files\SQLite_XLN_chatlogs.db';

//Opens the database the parameters are to avoid sql injections where a function is only outputing, not changing.
function OpenDB($mode=(SQLITE3_OPEN_READWRITE|SQLITE3_OPEN_CREATE))
{
    global $db, $dbfilename;
    $db = new SQLite3($dbfilename, $flags = $mode);
}

//closes the database
function CloseDB()
{
    global $db;
    $db->close();
}

//Resets the database entirely, drops all old tables, Creates new empty table. This is only used in a FULL reset
function ResetDB()
{
    global $db;
    OpenDB();
    $db->exec('DROP TABLE IF EXISTS "Chatlogs"');
    $db->exec('CREATE TABLE "Chatlogs" ( "ItemID" INTEGER NOT NULL UNIQUE, "ConversationID" TEXT NOT NULL, "Username" TEXT NOT NULL, "IssueID" INTEGER, "Message" TEXT, "TimeSent" TEXT NOT NULL, "IssueSolved" INTEGER, PRIMARY KEY("ItemID" AUTOINCREMENT) )');
    CloseDB();
}

//Enters each row of information
function InsertData($ConversationID, $Username, $IssueID, $Message, $IssueSolved)
{
    global $db; 
    OpenDB(); 
    $db->exec('BEGIN');
    $dt = new DateTimeImmutable($start, new DateTimeZone("Europe/London"));
    $TimeStamp = $dt->format("Y-m-d H:i:s");
    
    $sql = 'INSERT INTO Chatlogs (ConversationID, Username, IssueID, Message, TimeSent, IssueSolved) VALUES (:Convid, :Uname, :Issueid, :Msg, :Tsent, :Issueslv)';
    $stmt = $db->prepare($sql); //prepare the sql statement

    //give the values for the parameters
    $stmt->bindParam(':Convid', $ConversationID, SQLITE3_TEXT);
    $stmt->bindParam(':Uname', $Username, SQLITE3_TEXT); 
    $stmt->bindParam(':Issueid', $IssueID, SQLITE3_INTEGER);
    $stmt->bindParam(':Msg', $Message, SQLITE3_TEXT);
    $stmt->bindParam(':Tsent', TimeStamp, SQLITE3_TEXT);
    $stmt->bindParam(':Issueslv', $IssueSolved, SQLITE3_INTEGER);

    //execute the sql statement
    $stmt->execute();
    $db->exec("COMMIT");
    CloseDB();

    //the logic error catching
    if($stmt == FALSE){
        return "Error: something went wrong :(";
    }
}
/*
//Get the last conversation ID to know what the next convo ID will be. called only once.
function GetNewConvoID()
{  
    global $db;
    OpenDB();
    $sql1 = "SELECT COUNT(1) WHERE EXISTS (SELECT * FROM Chatlogs)";
    $stmt = $db->prepare($sql1);
    $result = $stmt->execute();
    if ($result == 0){
        $NewConvoID = 1;
        CloseDB();
    }//may have logic issues
    else{
        $sql2 = "SELECT TOP 1 ConversationID FROM Chatlogs ORDER BY ConversationID DESC";
        $stmt = $db->prepare($sql2);
        $result = $stmt->execute();
        $NewConvoID = $result + 1;
        //unsure if this ^^^ works, may need conversions.
        CloseDB();
    }

    return $NewConvoID; //CALL AT START OF EACH USE OF BOT TO START NEW ID
}
*/




?>