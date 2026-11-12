# Actividad 23
#He importado la librería datetime para guardar la hora y fecha exacta
from datetime import datetime

PASSWORD_CORRECTA = "secreto123"
intentos_fallidos = 0

print("=== SISTEMA DE SEGURIDAD ===")

while intentos_fallidos < 3:
    intento = input("Introduce la contraseña: ")
    
    if intento == PASSWORD_CORRECTA:
        print("¡Acceso concedido! Bienvenido al sistema.")
        break
    else:
        intentos_fallidos += 1
        print(f"Contraseña incorrecta. Te quedan {3 - intentos_fallidos} intento(s).")
        
        # Modo 'a' (append) para añadir líneas al log sin borrar las anteriores y añadiendolo al final
        with open("intentos.log", "a", encoding="utf-8") as f_log:
            ahora = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
            f_log.write(f"[{ahora}] Intento fallido con la clave: '{intento}'\n")

if intentos_fallidos == 3:
    print("ACCESO BLOQUEADO. Se han registrado demasiados intentos fallidos.")