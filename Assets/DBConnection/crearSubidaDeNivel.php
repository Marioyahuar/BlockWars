<?php
include_once "cors.php";
$newOrder = json_decode(file_get_contents("php://input"));
include_once "funciones.php";
$subidasPrevias = BuscarSubidaNivel($newOrder);
$creacionesTropaPrevias = BuscarCreacionTropa($newOrder);
if(count($subidasPrevias) > 0 || count($creacionesTropaPrevias) > 0) {
    $resultado = null;
    echo 'Esta ciudad ya esta subiendo de Nivel o tiene creacion de tropa pendiente';
}
else{
    $resultado = crearEnvioTropa($newOrder);
}

echo json_encode($resultado);