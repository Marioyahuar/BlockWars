<?php
include_once "cors.php";
$newOrder = json_decode(file_get_contents("php://input"));
include_once "funciones.php";
$subidasPrevias = BuscarSubidaNivel($newOrder);
$creacionesTropaPrevias = BuscarCreacionTropa($newOrder);
if(count($subidasPrevias) > 0){
    $resultado = 'SubiendoNivel';
} elseif(count($creacionesTropaPrevias)){
    $resultado = 'CreandoTropa';
} else{
    $resultado = crearEnvioTropa($newOrder);
}

echo $resultado;