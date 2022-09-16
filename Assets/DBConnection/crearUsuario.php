<?php
include_once "cors.php";
$newuser = json_decode(file_get_contents("php://input"));
include_once "funciones.php";
$check = obtenerIDUsuario($newuser->Wallet);
echo $check;
if($check != null){
    echo 'El usuario ya existe';
} else{
    $resultado = crearNuevoUsuario($newuser);
    $idusuario = obtenerIDUsuario($newuser->Wallet);
    $primerosvalles = obtenerPrimerValle();
    $randIndex = rand(0,count($primerosvalles)-1);
    $valleID = $primerosvalles[$randIndex]->IDMundoSpot;
    $newVillage = new stdClass();
    $newVillage->IDUsuario = $idusuario;
    $newVillage->IDMundoSpot = $valleID;
    $resultado = crearVilla($newVillage);
    echo json_encode($resultado);
}
