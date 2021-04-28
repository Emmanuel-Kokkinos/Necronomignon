using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Parent_Beast 
{

    void front_special();

    void back_special();

    void Play_SoundFX(string sound);

    string Beast_Name();

    void checkStatusEffect();

    void applyStatusEffect(string type);

}
