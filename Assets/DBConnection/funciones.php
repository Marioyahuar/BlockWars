<?php
//Funciones de conexión
function obtenerConexion(){
    $password = 'pix3lWars';
    $user = 'u822167468_pixelAdmin';
    $dbName = 'u822167468_PixelWarsM3D';
    $database = new PDO('mysql:host=localhost;dbname=' . $dbName, $user, $password);
    $database->query("set names utf8;");
    $database->setAttribute(PDO::ATTR_EMULATE_PREPARES, FALSE);
    $database->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $database->setAttribute(PDO::ATTR_DEFAULT_FETCH_MODE, PDO::FETCH_OBJ);
    return $database;
}

//Funciones de lectura
function obtenerIDUsuario($userWallet){
    $bd = obtenerConexion();
    $sentencia = $bd->prepare("SELECT IDUsuario FROM usuario WHERE Wallet=?");
    $sentencia->execute([$userWallet]);
    $result = $sentencia->fetchObject();
    return $result->IDUsuario;
}

function obtenerUserName($userID){
    $bd = obtenerConexion();
    $sentencia = $bd->prepare("SELECT UserName FROM usuario WHERE IDUsuario=?");
    $sentencia->execute([$userID]);
    $result = $sentencia->fetchObject();
    return $result->UserName;
}

function obtenerPrimerValle(){
    $bd = obtenerConexion();
    $sentencia = $bd->prepare("SELECT IDMundoSpot FROM mundo WHERE NivelMax=4 and TipoSpot='Valle'");
    $sentencia->execute();
    return $sentencia->fetchAll();
}

function obtenerRonda(){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("SELECT Ronda FROM servidor");
    $sentencia->execute();
    $result = $sentencia->fetchObject();
    return $result->Ronda;
}

function obtenerServerTime(){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("SELECT * FROM servidor");
    $sentencia->execute();
    $result = $sentencia->fetchObject();
    return $result;
}

function obtenerOrdenes($RondaActual){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("SELECT IDOrden_en_Curso, IDCiudadOrigen,IDCiudadDestino,IDTipo_Orden,Rondafin,TropasRetorno FROM orden_en_curso WHERE Rondafin=? and Estado=1");
    $sentencia->execute([$RondaActual]);
    return $sentencia->fetchAll();
}

function obtenerOrdenesCreacionTropa($RondaActual){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("SELECT IDOrden_en_Curso, IDCiudadOrigen,IDCiudadDestino,Rondafin,TropasSalida FROM orden_en_curso WHERE Rondafin=? and Estado=1 and IDTipo_Orden=1");
    $sentencia->execute([$RondaActual]);
    return $sentencia->fetchAll();
}

function obtenerOrdenesUpgradeCiudad($RondaActual){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("SELECT IDOrden_en_Curso, IDCiudadOrigen,IDCiudadDestino,Rondafin,TropasRetorno FROM orden_en_curso WHERE Rondafin=? and Estado=1 and IDTipo_Orden=2");
    $sentencia->execute([$RondaActual]);
    return $sentencia->fetchAll();
}

function obtenerOrdenesEnvioTropa($RondaActual){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("SELECT IDOrden_en_Curso, IDCiudadOrigen,IDCiudadDestino,Rondafin,TropasSalida FROM orden_en_curso WHERE Rondafin=? and Estado=1 and IDTipo_Orden=3");
    $sentencia->execute([$RondaActual]);
    return $sentencia->fetchAll();
}

function obtenerOrdenesAtaque($RondaActual){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("SELECT * FROM orden_en_curso WHERE Rondafin=? and Estado=1 and IDTipo_Orden=4");
    $sentencia->execute([$RondaActual]);
    return $sentencia->fetchAll();
}

function obtenerOrdenesColonizacion($RondaActual){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("SELECT IDOrden_en_Curso, IDCiudadOrigen,IDCiudadDestino,Rondafin,TropasSalida FROM orden_en_curso WHERE Rondafin=? and Estado=1 and IDTipo_Orden=5");
    $sentencia->execute([$RondaActual]);
    return $sentencia->fetchAll();
}

function obtenerOrdenesPorUsuario($userID){
    $bd = obtenerConexion();
    $ciudades = obtenerCiudadesPorUsuario($userID);
    $totalciudades = count($ciudades);
    $array = [];
    for($i = 0; $i < $totalciudades; $i++){
        $sentencia = $bd-> prepare("SELECT * FROM orden_en_curso WHERE Estado=1 and (IDCiudadOrigen = ? or IDCiudadDestino = ?)");
        $sentencia->execute([$ciudades[$i]->IDMundoSpot,$ciudades[$i]->IDMundoSpot]);
        $result = $sentencia->fetchAll();
        if($result != [])
        {
            array_push($array, $result);
        }
    }

    return $array;
}

function obtenerOrdenPorID($orderID){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("SELECT * FROM orden_en_curso WHERE IDOrden_en_Curso = ?");
    $sentencia->execute([$orderID]);
    return $sentencia->fetchObject();
}

function obtenerSpot($spotID){
    $bd = obtenerConexion();
    $sentencia = $bd->prepare("SELECT * FROM mundo WHERE Estado=1 and IDMundoSpot = ?");
    $sentencia->execute([$spotID]);
    return $sentencia->fetchObject();
}

function obtenerMundo(){
    $bd = obtenerConexion();
    $sentencia = $bd->prepare("SELECT * FROM mundo WHERE Estado=1");
    $sentencia->execute();
    return $sentencia->fetchAll();
}

function obtenerCiudadesPorUsuario($userID){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("SELECT * FROM mundo WHERE IDUsuario=? and Estado=1");
    $sentencia->execute([$userID]);
    return $sentencia->fetchAll();
}

function obtenerReportesPorUsuario($userID){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("SELECT * FROM Reporte WHERE IDAtacante=? or IDDefensor=? ORDER BY IDReporte DESC");
    $sentencia->execute([$userID,$userID]);
    return $sentencia->fetchAll();
}

function ReiniciarServer(){
    
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("DELETE FROM Reporte");
    $sentencia1 = $bd-> prepare("alter table Reporte AUTO_INCREMENT=1");
    $sentencia2 = $bd-> prepare("DELETE FROM orden_en_curso");
    $sentencia3 = $bd-> prepare("alter table orden_en_curso AUTO_INCREMENT=1");
    $sentencia4 = $bd-> prepare("UPDATE servidor SET Ronda = '0' WHERE servidor.IDServer = 1");
    $sentencia5 = $bd-> prepare("UPDATE mundo SET Nombre= null,NivelActual=null,Tropas=null,TipoSpot='Valle',IDUsuario=Null WHERE TipoSpot='Ciudad'");
    $sentencia6 = $bd-> prepare("UPDATE mundo SET Tropas=2 WHERE TipoSpot='Barbaros'");
    $sentencia7 = $bd-> prepare("DELETE FROM usuario;");
    $sentencia8 = $bd-> prepare("alter table usuario AUTO_INCREMENT=1");
    $sentencia -> execute();
    $sentencia1 -> execute();
    $sentencia2 -> execute();
    $sentencia3 -> execute();
    $sentencia4 -> execute();
    $sentencia5 -> execute();
    $sentencia6 -> execute();
    $sentencia7 -> execute();
    $sentencia8 -> execute();
    $result = 'Server reiniciado';
    return  $result;

    /*
    DELETE FROM Reporte;
alter table Reporte AUTO_INCREMENT=1;

DELETE FROM orden_en_curso;
alter table orden_en_curso AUTO_INCREMENT=1;


UPDATE servidor SET Ronda = '0' WHERE servidor.IDServer = 1;


UPDATE mundo SET Nombre= null,NivelActual=null,Tropas=null,TipoSpot='Valle',IDUsuario=Null WHERE TipoSpot='Ciudad';
UPDATE mundo SET Tropas=2 WHERE TipoSpot='Barbaros';

DELETE FROM usuario;
alter table usuario AUTO_INCREMENT=1;
    */
}

function BuscarAtaque($BuscarAtaque){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("SELECT * FROM orden_en_curso WHERE IDTipo_Orden=4 AND Rondafin=? AND IDCiudadDestino=? AND Estado=1");
    $sentencia->execute([$BuscarAtaque->Rondafin,$BuscarAtaque->IDCiudadDestino]);
    return $sentencia->fetchAll();
}

function BuscarCreacionTropa($ordenCreacion){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("SELECT * FROM orden_en_curso WHERE IDTipo_Orden=1 AND Rondafin=? AND IDCiudadDestino=? AND Estado=1");
    $sentencia->execute([$ordenCreacion->Rondafin,$ordenCreacion->IDCiudadDestino]);
    return $sentencia->fetchAll();
}

function BuscarSubidaNivel($ordenSubidaNivel){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("SELECT * FROM orden_en_curso WHERE IDTipo_Orden=2 AND Rondafin=? AND IDCiudadDestino=? AND Estado=1");
    $sentencia->execute([$ordenSubidaNivel->Rondafin,$ordenSubidaNivel->IDCiudadDestino]);
    return $sentencia->fetchAll();
}

//Funciones de escritura
function inicializarServer($horainicio){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("UPDATE servidor SET TiempoInicializado = ? where IDServer=1");
    return $sentencia->execute([$horainicio]); 
}

function crearNuevoUsuario($newuser){
    $bd = obtenerConexion();
    $sentencia = $bd->prepare("INSERT INTO usuario(Username, Wallet) VALUES (?, ?)");
    return $sentencia->execute([$newuser->Username, $newuser->Wallet]);
}

function crearReporte($newReporte){
    $bd = obtenerConexion();
    $sentencia = $bd->prepare("INSERT INTO Reporte(IDOrdenEncurso,Resultado,IDAtacante,IDDefensor,PerdidaDefensa,PerdidaAtaque,Ronda,Defensores) VALUES (?,?,?,?,?,?,?,?)");
    return $sentencia->execute([$newReporte->IDOrdenEncurso,$newReporte->Resultado,$newReporte->IDAtacante,$newReporte->IDDefensor,$newReporte->PerdidaDefensa,$newReporte->PerdidaAtaque,$newReporte->Ronda,$newReporte->Defensores]);
}

function crearSlot($newspot){
    $bd = obtenerConexion();
    $sentencia = $bd->prepare("INSERT INTO mundo(Ubicacion, NivelMax, TipoSpot, IDMapa) VALUES (?, ?, ?, ?)");
    return $sentencia->execute([$newspot->Ubicacion, $newspot->NivelMax, $newspot->TipoSpot, $newspot-> IDMapa]);
}

function crearVilla($newVillage){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("UPDATE mundo SET Nombre= 'Village', NivelActual=1, Tropas = 5, TipoSpot='Ciudad', IDUsuario=? WHERE IDMundoSpot=?");
    return $sentencia->execute([$newVillage->IDUsuario, $newVillage->IDMundoSpot]);
}

function crearOrden($nuevaOrden){
    $bd = obtenerConexion();
    $sentencia = $bd->prepare("INSERT INTO orden_en_curso(IDTipo_Orden, IDCiudadOrigen,IDCiudadDestino,RondaInicio,TropasSalida,Rondafin,TropasRetorno) VALUES (?,?,?,?,?,?,?)");
    return $sentencia->execute([$nuevaOrden->IDTipo_Orden,$nuevaOrden->IDCiudadOrigen,$nuevaOrden->IDCiudadDestino,$nuevaOrden->RondaInicio,$nuevaOrden->TropasSalida,$nuevaOrden->Rondafin,$nuevaOrden->TropasRetorno]);
}

function crearEnvioTropa($nuevaOrden){ //Puede ser ataque o defensa o // subidaDeNivel
    $ciudadOrigen = obtenerSpot($nuevaOrden->IDCiudadOrigen);
    if($ciudadOrigen->Tropas < $nuevaOrden->TropasSalida){
        echo 'No tienes tropas suficientes para enviar esta mision';
        $result = null;
    } else{
        $result = crearOrden($nuevaOrden);
        //Descontar las tropas
        $bd = obtenerConexion();
        $sentencia = $bd->prepare("UPDATE mundo SET Tropas = (Tropas - ?) WHERE IDMundoSpot=?");
        $sentencia->execute([$nuevaOrden->TropasSalida,$nuevaOrden->IDCiudadOrigen]);
    }
    
    return $result;
}

function pasarRonda(){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("UPDATE servidor SET Ronda= Ronda+1,TiempoRonda= now()");
    return $sentencia->execute();

}

function agregarTropas($nuevasTropas){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("UPDATE mundo SET Tropas=(Tropas + ?) where IDMundoSpot=? ");
    return $sentencia->execute([$nuevasTropas->Tropas, $nuevasTropas->IDMundoSpot]); 
}

function EliminarTropasPerdedores($IDMundoSpot){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("UPDATE mundo SET Tropas=0 where IDMundoSpot=? ");
    return $sentencia->execute([$IDMundoSpot]); 
}

function updateTropas($tropas){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("UPDATE mundo SET Tropas=? where IDMundoSpot=? ");
    return $sentencia->execute([$tropas->Tropas,$tropas->IDMundoSpot]); 
}

function UpgradeCiudad($IDMundoSpot){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("UPDATE mundo SET NivelActual=(NivelActual + 1) where IDMundoSpot=? ");
    return $sentencia->execute([$IDMundoSpot]);
}

function CiudadStanby($ciudadDestino){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("UPDATE mundo SET Estado=? where IDMundoSpot=? ");
    return $sentencia->execute([$nuevaOrden->IDTipo_Orden,$nuevaOrden->IDCiudadDestino]);
}

function CalcularDefensa($ciudadDestino){
    $TropasDefensoras=$ciudadDestino->Tropas; //25
    $NivelDefensa=$ciudadDestino->NivelActual; 
    $dfPower=$TropasDefensoras*30; //750
    $BonusDF=0;
    switch ($NivelDefensa) {
        case 1:
            $BonusDF=0.02;
            break;
        case 2:
            $BonusDF=0.05;
            break;
        case 3:
            $BonusDF=0.08;
            break;
        case 4:
            $BonusDF=0.10;
            break;
        case 5:
            $BonusDF=0.13;
            break;
        case 6:
            $BonusDF=0.16;
            break;
        case 7:
            $BonusDF=0.19;
            break;    
    }
    $BonusDF=$BonusDF*$dfPower; //15
   return $dfPower=$dfPower+$BonusDF; //787.5
}

function CalcularBajas($ppgg,$cantidadTropas){
    $porcentajeTropaPerdida = pow($ppgg , 1.5); 
    $CantidadMuertes = $cantidadTropas * $porcentajeTropaPerdida; //18.7
    $bajas=floor($CantidadMuertes); //18
    return $bajas;
}

function EjecutarBatalla($atacantes,$defensores,$orden){
    echo 'Ejecucionbatalla';
    $ciudadOrigen = obtenerSpot($orden->IDCiudadOrigen); //$ciudadOrigen = ciudad(108)
    //se calcula los puntos de ataque
    $AtaqueTotal=$atacantes*40; //800 //Para la fase beta las tropas tienen 40 atk, 30 def
    //se calcula los puntos de defensa
    $ciudadDestino = obtenerSpot($orden->IDCiudadDestino); //#ciudadDestino = ciudad(275)
    $defensaTotal=CalcularDefensa($ciudadDestino); //765
    

    if($AtaqueTotal > $defensaTotal){//Gana Atacante
        $ganador = $ciudadOrigen->IDUsuario;
        $ppgg = $defensaTotal/$AtaqueTotal; //0.95625
        echo $ppgg;
        $Bajas=CalcularBajas($ppgg,$atacantes); //18
        $atacantesSobrevivientes = $atacantes - $Bajas; //2
        $defensoresSobrevivientes = 0;
        EliminarTropasPerdedores($ciudadDestino->IDMundoSpot);
        //Generar nueva orden
        $nuevaOrden = new stdClass();
        $nuevaOrden->IDTipo_Orden = 3;
        $nuevaOrden->IDCiudadOrigen = $orden->IDCiudadDestino;
        $nuevaOrden->IDCiudadDestino = $orden->IDCiudadOrigen;
        $nuevaOrden->RondaInicio = $orden->Rondafin + 1;
        $nuevaOrden->TropasSalida = $atacantesSobrevivientes + floor($defensores/2); //+ floor($defensores/2)
        $nuevaOrden->Rondafin = $orden->Rondafin + 1 + ($orden->Rondafin - $orden->RondaInicio);
        $nuevaOrden->TropasRetorno = 0;
        crearOrden($nuevaOrden);
    }
    else{//Gana Defensor
        $ganador = $ciudadDestino->IDUsuario;
        $ppgg = $AtaqueTotal/$defensaTotal;
        $Bajas=CalcularBajas($ppgg,$defensores);
        $atacantesSobrevivientes = 0;
        $defensoresSobrevivientes = $defensores - $Bajas;
        $defensorestropas = new stdClass();
        $defensorestropas->Tropas = $defensoresSobrevivientes;
        $defensorestropas->IDMundoSpot = $orden->IDCiudadDestino;
        updateTropas($defensorestropas);
    }     
    
    //Generar reporte

    //IDOrdenEncurso,Resultado,IDAtacante,IDDefensor,PerdidaDefensa,PerdidaAtaque)
    $newReporte = new stdClass();
    $newReporte->IDOrdenEncurso = $orden->IDOrden_en_Curso;
    $newReporte->Resultado = $ganador;
    $newReporte->IDAtacante = $ciudadOrigen->IDUsuario;
    $newReporte->IDDefensor = $ciudadDestino->IDUsuario;
    $newReporte->PerdidaDefensa = $defensores - $defensoresSobrevivientes;
    $newReporte->PerdidaAtaque = $atacantes - $atacantesSobrevivientes;
    $newReporte->Ronda = obtenerRonda();
    $newReporte->Defensores = $defensores;
    $result = crearReporte($newReporte); 
    return $result;
}

function EjecutarColonizacion($ordenColonizacion) {
    
    $ciudadDestino = obtenerSpot($ordenColonizacion->IDCiudadDestino);
    $ciudadOrigen = obtenerSpot($ordenColonizacion->IDCiudadOrigen);
    if($ciudadDestino->TipoSpot == 'Valle'){
        //Ejecuta colonización
        $newVilla = new stdClass();
        $newVilla->IDUsuario = $ciudadOrigen->IDUsuario;
        $newVilla->IDMundoSpot = $ordenColonizacion->IDCiudadDestino;
        crearVilla($newVilla);
    } else{
        //Crear orden de retorno
        echo 'La ciudad ya fue colonizada';
        $nuevaOrden = new stdClass();
        $nuevaOrden->IDTipo_Orden = 3;
        $nuevaOrden->IDCiudadOrigen = $ordenColonizacion->IDCiudadDestino;
        $nuevaOrden->IDCiudadDestino = $ordenColonizacion->IDCiudadOrigen;
        $nuevaOrden->RondaInicio = $ordenColonizacion->Rondafin;
        $nuevaOrden->TropasSalida = $ordenColonizacion->TropasSalida; //+ floor($defensores/2)
        $nuevaOrden->Rondafin = $ordenColonizacion->Rondafin + ($ordenColonizacion->Rondafin - $ordenColonizacion->RondaInicio);
        $nuevaOrden->TropasRetorno = 0;
        crearOrden($nuevaOrden);
    }
}

function finalizarOrden($ordenID){
    $bd = obtenerConexion();
    $sentencia = $bd-> prepare("UPDATE orden_en_curso SET Estado=0 where IDOrden_en_Curso=?");
    return $sentencia->execute([$ordenID]); 
}

function RestoreBarbarians($RondaActual){
    $bd = obtenerConexion();
    $mod = $RondaActual % 10;
    if($mod == 0){
        $sentencia = $bd-> prepare("UPDATE mundo SET Tropas=2 where TipoSpot = 'Barbaros' and Tropas=0");
        return $sentencia->execute(); 
    } else{
        return 'No es hora de reiniciar';
    }
    
}

