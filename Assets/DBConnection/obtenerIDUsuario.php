<?php
include_once "cors.php";
$userWallet = $_GET["userWallet"];
include_once "funciones.php";
$usuario = obtenerIDUsuario($userWallet);
echo json_encode($usuario);