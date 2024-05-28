using Sproto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SprotoType
{
    public class package : SprotoTypeBase
    {
        private static int max_field_count = 2;


        private Int64 _type; // tag 0
        public Int64 type
        {
            get { return _type; }
            set { base.has_field.set_field(0, true); _type = value; }
        }
        public bool HasType
        {
            get { return base.has_field.has_field(0); }
        }

        private Int64 _session; // tag 1
        public Int64 session
        {
            get { return _session; }
            set { base.has_field.set_field(1, true); _session = value; }
        }
        public bool HasSession
        {
            get { return base.has_field.has_field(1); }
        }

        public package() : base(max_field_count) { }

        public package(byte[] buffer) : base(max_field_count, buffer)
        {
            this.decode();
        }

        protected override void decode()
        {
            int tag = -1;
            while (-1 != (tag = base.deserialize.read_tag()))
            {
                switch (tag)
                {
                    case 0:
                        this.type = base.deserialize.read_integer();
                        break;
                    case 1:
                        this.session = base.deserialize.read_integer();
                        break;
                    default:
                        base.deserialize.read_unknow_data();
                        break;
                }
            }
        }

        public override int encode(SprotoStream stream)
        {
            base.serialize.open(stream);

            if (base.has_field.has_field(0))
            {
                base.serialize.write_integer(this.type, 0);
            }

            if (base.has_field.has_field(1))
            {
                base.serialize.write_integer(this.session, 1);
            }

            return base.serialize.close();
        }
    }
}
