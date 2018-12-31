Namespace DAoCServer
    <Serializable()> _
    Public Enum DAOCCharacterClass
        Unknown
        Armsman
        Cabalist
        Cleric
        Friar
        Infiltrator
        Mercenary
        Minstrel
        Paladin
        Scout
        Sorcerer
        Theurgist
        Wizard
        Bard
        Blademaster
        Champion
        Druid
        Eldritch
        Enchanter
        Hero
        Mentalist
        Nightshade
        Ranger
        Warden
        Berserker
        Healer
        Hunter
        Runemaster
        Shadowblade
        Shaman
        Skald
        Spiritmaster
        Thane
        Warrior
    End Enum

    <Serializable()> _
    Public Enum eSpellStatus
        Failed
        Success
        StartSong
    End Enum

    <Serializable()> _
    Public NotInheritable Class SpellEffectAnimation
        Private mCasterID As Integer
        Private mSpellCastID As Integer
        Private mTargetID As Integer
        Private mTimeToHit As Integer
        Private mSpellStatus As eSpellStatus

        Public Property CasterID() As Integer
            Get
                Return mCasterID
            End Get
            Set(ByVal Value As Integer)
                mCasterID = Value
            End Set
        End Property

        Public Property SpellCastID() As Integer
            Get
                Return mSpellCastID
            End Get
            Set(ByVal Value As Integer)
                mSpellCastID = Value
            End Set
        End Property

        Public Property TargetID() As Integer
            Get
                Return mTargetID
            End Get
            Set(ByVal Value As Integer)
                mTargetID = Value
            End Set
        End Property

        Public Property TimeToHit() As Integer
            Get
                Return mTimeToHit
            End Get
            Set(ByVal Value As Integer)
                mTimeToHit = Value
            End Set
        End Property

        Public Property SpellStatus() As eSpellStatus
            Get
                Return mSpellStatus
            End Get
            Set(ByVal Value As eSpellStatus)
                mSpellStatus = Value
            End Set
        End Property
    End Class

    <Serializable()> _
   Public NotInheritable Class SpellCast
        Private mCasterID As Integer
        Private mSpellCastID As Integer
        Private mTimeToCast As Integer

        Public Property CasterID() As Integer
            Get
                Return mCasterID
            End Get
            Set(ByVal Value As Integer)
                mCasterID = Value
            End Set
        End Property

        Public Property SpellCastID() As Integer
            Get
                Return mSpellCastID
            End Get
            Set(ByVal Value As Integer)
                mSpellCastID = Value
            End Set
        End Property

        Public Property TimeToCast() As Integer
            Get
                Return mTimeToCast
            End Get
            Set(ByVal Value As Integer)
                mTimeToCast = Value
            End Set
        End Property
    End Class

    <Serializable()> _
    Public NotInheritable Class DAOCPet
        Private mID As Integer
        Private mType As Integer
        Private mBuffs As ArrayList
        Private mPetState As psState
        Private mPetPosition As ppPosition

        Public Enum psState
            aggressive = 1
            defensive = 2
            passive = 3
        End Enum

        Public Enum ppPosition
            Follow = 1
            Stay = 2
            GoTarget = 3
            Here = 4
        End Enum

        Public Property Type() As Integer
            Get
                Return mType
            End Get
            Set(ByVal Value As Integer)
                mType = Value
            End Set
        End Property

        Public Property PetPosition() As ppPosition
            Get
                Return mPetPosition
            End Get
            Set(ByVal Value As ppPosition)
                mPetPosition = Value
            End Set
        End Property

        Public Property ID() As Integer
            Get
                Return mID
            End Get
            Set(ByVal Value As Integer)
                mID = Value
            End Set
        End Property

        Public Property Buffs() As ArrayList
            Get
                Return mBuffs
            End Get
            Set(ByVal Value As ArrayList)
                mBuffs = Value
            End Set
        End Property

        Public Sub New()
            mBuffs = New ArrayList
        End Sub

        Protected Overrides Sub Finalize()
            mBuffs = Nothing
            MyBase.Finalize()
        End Sub

        Public Property PetState() As psState
            Get
                Return mPetState
            End Get
            Set(ByVal Value As psState)
                mPetState = Value
            End Set
        End Property
    End Class
End Namespace


